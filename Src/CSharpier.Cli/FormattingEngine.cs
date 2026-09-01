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

internal sealed record PhysicalPath(
    string ActualPath,
    string SuppliedPath,
    string DirectoryName,
    bool IsFile
);

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
    private static readonly StringComparer PathComparer = OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;

    public async Task FormatPhysicalPaths(
        IReadOnlyList<PhysicalPath> paths,
        CancellationToken cancellationToken
    )
    {
        if (!writer.SupportsParallelWrites)
        {
            await foreach (var file in this.EnumerateFiles(paths, cancellationToken))
            {
                await this.FormatFile(
                    file.ActualPath,
                    file.SuppliedPath,
                    checkIsIgnored: true,
                    file.WarnForUnsupported,
                    file.IgnoreRootDirectory,
                    ct => FileToFormatInfo.CreateFromFileSystem(file.ActualPath, fileSystem, ct),
                    cancellationToken
                );
            }
            return;
        }

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = DefaultMaxDegreeOfParallelism,
            CancellationToken = cancellationToken,
        };

        await Parallel.ForEachAsync(
            this.EnumerateFiles(paths, cancellationToken),
            parallelOptions,
            async (file, formattingToken) =>
                await this.FormatFile(
                    file.ActualPath,
                    file.SuppliedPath,
                    checkIsIgnored: true,
                    file.WarnForUnsupported,
                    file.IgnoreRootDirectory,
                    ct => FileToFormatInfo.CreateFromFileSystem(file.ActualPath, fileSystem, ct),
                    formattingToken
                )
        );
    }

    private async IAsyncEnumerable<PhysicalFile> EnumerateFiles(
        IReadOnlyList<PhysicalPath> paths,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var seenPaths = new HashSet<string>(PathComparer);
        var explicitPaths = new Lazy<Dictionary<string, PhysicalPath>>(() =>
            paths
                .Where(path => path.IsFile)
                .DistinctBy(path => path.ActualPath, PathComparer)
                .ToDictionary(path => path.ActualPath, PathComparer)
        );

        foreach (var path in paths)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (path.IsFile)
            {
                if (seenPaths.Add(path.ActualPath))
                {
                    yield return new PhysicalFile(
                        path.ActualPath,
                        path.SuppliedPath,
                        WarnForUnsupported: true,
                        path.DirectoryName
                    );
                }
                continue;
            }

            await foreach (
                var file in this.EnumerateNonignoredFiles(
                    path.ActualPath,
                    path.DirectoryName,
                    cancellationToken
                )
            )
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (seenPaths.Add(file))
                {
                    yield return explicitPaths.Value.TryGetValue(file, out var explicitPath)
                        ? new PhysicalFile(
                            file,
                            explicitPath.SuppliedPath,
                            WarnForUnsupported: true,
                            explicitPath.DirectoryName
                        )
                        : new PhysicalFile(
                            file,
                            path.SuppliedPath + file[path.ActualPath.Length..],
                            WarnForUnsupported: false,
                            path.DirectoryName
                        );
                }
            }
        }
    }

    private async IAsyncEnumerable<string> EnumerateNonignoredFiles(
        string directoryPath,
        string ignoreRootDirectory,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        foreach (var file in fileSystem.Directory.EnumerateFiles(directoryPath))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return file;
        }

        foreach (var subdirectory in fileSystem.Directory.EnumerateDirectories(directoryPath))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (
                await optionsProvider.IsDirectoryIgnoredAsync(
                    subdirectory,
                    ignoreRootDirectory,
                    cancellationToken
                )
            )
            {
                continue;
            }

            await foreach (
                var file in this.EnumerateNonignoredFiles(
                    subdirectory,
                    ignoreRootDirectory,
                    cancellationToken
                )
            )
            {
                yield return file;
            }
        }
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
            ignoreRootDirectory: null,
            _ => Task.FromResult(fileToFormatInfo),
            cancellationToken
        );
    }

    private async Task FormatFile(
        string actualFilePath,
        string originalFilePath,
        bool checkIsIgnored,
        bool warnForUnsupported,
        string? ignoreRootDirectory,
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
                && await optionsProvider.IsFileIgnoredAsync(
                    actualFilePath,
                    ignoreRootDirectory,
                    cancellationToken
                )
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

        if (formattingCache.CanSkipFormatting(fileToFormatInfo, printerOptions))
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
        formattingCache.CacheResult(codeFormattingResult.Code, fileToFormatInfo, printerOptions);
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

    private sealed record PhysicalFile(
        string ActualPath,
        string SuppliedPath,
        bool WarnForUnsupported,
        string IgnoreRootDirectory
    );
}
