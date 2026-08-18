using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Text;
using CSharpier.Cli.Options;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using CSharpier.Core.Utilities;
using CSharpier.Core.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class FormattingEngine(
    IFormattedFileWriter writer,
    OptionsProvider optionsProvider,
    IFormattingCache formattingCache,
    CommandLineOptions commandLineOptions,
    IFileSystem fileSystem,
    ILogger logger,
    CommandLineFormatterResult result
)
{
    private static readonly int DefaultMaxDegreeOfParallelism = Environment.ProcessorCount * 2;

    public async Task FormatDirectory(
        string actualPath,
        string suppliedPath,
        CancellationToken cancellationToken
    )
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = DefaultMaxDegreeOfParallelism,
            CancellationToken = cancellationToken,
        };

        try
        {
            await Parallel.ForEachAsync(
                this.EnumerateNonignoredFiles(actualPath, cancellationToken),
                parallelOptions,
                async (file, formattingToken) =>
                {
                    var relativePath = suppliedPath + file[actualPath.Length..];
                    await this.FormatPhysicalFile(file, relativePath, false, formattingToken);
                }
            );
        }
        catch (OperationCanceledException ex)
        {
            if (ex.CancellationToken != cancellationToken)
            {
                throw;
            }
        }
    }

    private async IAsyncEnumerable<string> EnumerateNonignoredFiles(
        string directoryPath,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        foreach (var file in fileSystem.Directory.EnumerateFiles(directoryPath))
        {
            yield return file;
        }

        foreach (var subdirectory in fileSystem.Directory.EnumerateDirectories(directoryPath))
        {
            if (await optionsProvider.IsDirectoryIgnoredAsync(subdirectory, cancellationToken))
            {
                continue;
            }

            await foreach (
                var file in this.EnumerateNonignoredFiles(subdirectory, cancellationToken)
            )
            {
                yield return file;
            }
        }
    }

    public Task FormatPhysicalFile(
        string actualPath,
        string suppliedPath,
        bool warnForUnsupported,
        CancellationToken cancellationToken
    )
    {
        return this.FormatFile(
            actualPath,
            suppliedPath,
            checkIsIgnored: true,
            warnForUnsupported,
            ct => FileToFormatInfo.CreateFromFileSystem(actualPath, fileSystem, ct),
            cancellationToken
        );
    }

    public Task FormatStandardInputFile(
        FileToFormatInfo fileToFormatInfo,
        string originalPath,
        bool checkIsIgnored,
        CancellationToken cancellationToken
    )
    {
        return this.FormatFile(
            fileToFormatInfo.Path,
            originalPath,
            checkIsIgnored,
            warnForUnsupported: false,
            _ => Task.FromResult(fileToFormatInfo),
            cancellationToken
        );
    }

    private async Task FormatFile(
        string actualFilePath,
        string originalFilePath,
        bool checkIsIgnored,
        bool warnForUnsupported,
        Func<CancellationToken, Task<FileToFormatInfo>> getFileToFormatInfo,
        CancellationToken cancellationToken
    )
    {
        if (
            (
                !commandLineOptions.IncludeGenerated
                && GeneratedCodeUtilities.IsGeneratedCodeFile(actualFilePath)
            )
            || (
                checkIsIgnored
                && await optionsProvider.IsFileIgnoredAsync(actualFilePath, cancellationToken)
            )
        )
        {
            return;
        }

        var printerOptions = await optionsProvider.GetPrinterOptionsForAsync(
            actualFilePath,
            cancellationToken
        );

        if (printerOptions is { Formatter: not Formatter.Unknown })
        {
            printerOptions.IncludeGenerated = commandLineOptions.IncludeGenerated;

            var fileToFormatInfo = await getFileToFormatInfo(cancellationToken);

            var fileIssueLogger = new FileIssueLogger(
                originalFilePath,
                logger,
                commandLineOptions.LogFormat
            );

            logger.LogDebug(
                commandLineOptions.Check
                    ? $"Checking - {originalFilePath}"
                    : $"Formatting - {originalFilePath}"
            );

            await this.PerformFormattingSteps(
                fileToFormatInfo,
                fileIssueLogger,
                printerOptions,
                cancellationToken
            );
        }
        else if (warnForUnsupported)
        {
            var fileIssueLogger = new FileIssueLogger(
                originalFilePath,
                logger,
                logFormat: commandLineOptions.LogFormat
            );
            fileIssueLogger.WriteWarning("Is an unsupported file type.");
        }
    }

    public async Task PerformFormattingSteps(
        FileToFormatInfo fileToFormatInfo,
        FileIssueLogger fileIssueLogger,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        if (fileToFormatInfo.FileContents.Length == 0)
        {
            return;
        }

        Interlocked.Increment(ref result.Files);

        if (formattingCache.CanSkipFormatting(fileToFormatInfo))
        {
            Interlocked.Increment(ref result.CachedFiles);
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        CodeFormatterResult codeFormattingResult;

        try
        {
            codeFormattingResult = await CodeFormatter.FormatAsync(
                fileToFormatInfo.FileContents,
                printerOptions,
                cancellationToken
            );
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            fileIssueLogger.WriteError("Threw exception while formatting.", ex);
            Interlocked.Increment(ref result.ExceptionsFormatting);
            return;
        }

        if (codeFormattingResult.ErrorDiagnostics.Any())
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("Was not formatted due to syntax errors.");
            foreach (var message in codeFormattingResult.ErrorDiagnostics)
            {
                errorMessage.AppendLine(message.ToString());
            }

            if (!commandLineOptions.SyntaxErrorsAsWarnings)
            {
                fileIssueLogger.WriteError(errorMessage.ToString());
            }
            else
            {
                fileIssueLogger.WriteWarning(errorMessage.ToString());
            }

            Interlocked.Increment(ref result.FailedCompilation);
            return;
        }

        if (!string.IsNullOrEmpty(codeFormattingResult.WarningMessage))
        {
            fileIssueLogger.WriteWarning(codeFormattingResult.WarningMessage);
            return;
        }

        if (!string.IsNullOrEmpty(codeFormattingResult.FailureMessage))
        {
            fileIssueLogger.WriteError(codeFormattingResult.FailureMessage);
            return;
        }

        if (!commandLineOptions.SkipValidation)
        {
            await this.ValidateFormatting(
                fileToFormatInfo,
                codeFormattingResult,
                printerOptions,
                fileIssueLogger,
                cancellationToken
            );
        }

        if (
            commandLineOptions is { Check: true, WriteStdout: false }
            && codeFormattingResult.Code != fileToFormatInfo.FileContents
        )
        {
            var difference = StringDiffer.PrintFirstDifference(
                codeFormattingResult.Code,
                fileToFormatInfo.FileContents
            );
            var message = $"Was not formatted.\n{difference}\n";
            if (commandLineOptions.UnformattedAsWarnings)
            {
                fileIssueLogger.WriteWarning(message);
            }
            else
            {
                fileIssueLogger.WriteError(message);
            }

            Interlocked.Increment(ref result.UnformattedFiles);
        }

        writer.WriteResult(codeFormattingResult, fileToFormatInfo);
        formattingCache.CacheResult(codeFormattingResult.Code, fileToFormatInfo);
    }

    private async Task ValidateFormatting(
        FileToFormatInfo fileToFormatInfo,
        CodeFormatterResult codeFormattingResult,
        PrinterOptions printerOptions,
        FileIssueLogger fileIssueLogger,
        CancellationToken cancellationToken
    )
    {
        IFormattingValidator? formattingValidator = null;

        if (
            printerOptions.Formatter is Formatter.CSharp or Formatter.CSharpScript
            && fileToFormatInfo.FileContents != codeFormattingResult.Code
        )
        {
            var sourceCodeKind =
                printerOptions.Formatter is Formatter.CSharpScript
                    ? SourceCodeKind.Script
                    : SourceCodeKind.Regular;

            var syntaxNodeComparer = new SyntaxNodeComparer(
                fileToFormatInfo.FileContents,
                codeFormattingResult.Code,
                codeFormattingResult.ReorderedModifiers,
                codeFormattingResult.ReorderedUsingsWithDisabledText,
                codeFormattingResult.MovedTrailingTrivia,
                sourceCodeKind,
                cancellationToken
            );

            formattingValidator = new CSharpFormattingValidator(syntaxNodeComparer);
        }
        else if (printerOptions.Formatter is Formatter.XML)
        {
            formattingValidator = new XmlFormattingValidator(
                fileToFormatInfo.FileContents,
                codeFormattingResult.Code
            );
        }
        else
        {
            // TODO log error?
        }

        if (formattingValidator is not null)
        {
            try
            {
                var validatorResult = await formattingValidator.ValidateAsync(cancellationToken);
                if (validatorResult.Failed)
                {
                    Interlocked.Increment(ref result.FailedFormattingValidation);
                    fileIssueLogger.WriteError(
                        $"Failed formatting validation.{(string.IsNullOrEmpty(validatorResult.FailureMessage) ? null : "\n" + validatorResult.FailureMessage)}"
                    );
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref result.ExceptionsValidatingSource);

                fileIssueLogger.WriteError(
                    "Failed with exception during syntax tree validation.",
                    ex
                );
            }
        }
    }
}
