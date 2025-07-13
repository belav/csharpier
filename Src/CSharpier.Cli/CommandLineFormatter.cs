using System.Diagnostics;
using System.IO.Abstractions;
using System.Text;
using CSharpier.Cli.Options;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using CSharpier.Core.Utilities;
using CSharpier.Core.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal static class CommandLineFormatter
{
    public static async Task<int> Format(
        CommandLineOptions commandLineOptions,
        IFileSystem fileSystem,
        IConsole console,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var timestamp = Stopwatch.GetTimestamp();
            var commandLineFormatterResult = new CommandLineFormatterResult();

            if (commandLineOptions.StandardInFileContents != null)
            {
                var directoryOrFilePath = commandLineOptions.DirectoryOrFilePaths[0];
                var directoryPath = fileSystem.Directory.Exists(directoryOrFilePath)
                    ? directoryOrFilePath
                    : fileSystem.Path.GetDirectoryName(directoryOrFilePath);
                var filePath =
                    directoryOrFilePath != directoryPath
                        ? directoryOrFilePath
                        : Path.Combine(directoryPath, "StdIn.cs");

                ArgumentNullException.ThrowIfNull(directoryPath);

                var fileToFormatInfo = FileToFormatInfo.Create(
                    filePath,
                    commandLineOptions.StandardInFileContents,
                    console.InputEncoding
                );

                var optionsProvider = await OptionsProvider.Create(
                    directoryPath,
                    commandLineOptions.ConfigPath,
                    commandLineOptions.IgnorePath,
                    fileSystem,
                    logger,
                    cancellationToken
                );

                if (
                    (
                        commandLineOptions.IncludeGenerated
                        || !GeneratedCodeUtilities.IsGeneratedCodeFile(filePath)
                    ) && !await optionsProvider.IsIgnoredAsync(filePath, cancellationToken)
                )
                {
                    var fileIssueLogger = new FileIssueLogger(
                        commandLineOptions.OriginalDirectoryOrFilePaths[0],
                        logger,
                        commandLineOptions.LogFormat
                    );

                    var printerOptions = await optionsProvider.GetPrinterOptionsForAsync(
                        filePath,
                        cancellationToken
                    );
                    if (printerOptions is { Formatter: not Formatter.Unknown })
                    {
                        printerOptions.IncludeGenerated = commandLineOptions.IncludeGenerated;

                        await PerformFormattingSteps(
                            fileToFormatInfo,
                            new StdOutFormattedFileWriter(console),
                            commandLineFormatterResult,
                            fileIssueLogger,
                            printerOptions,
                            commandLineOptions,
                            FormattingCacheFactory.NullCache,
                            cancellationToken
                        );
                    }
                }
            }
            else
            {
                var result = await FormatPhysicalFiles(
                    commandLineFormatterResult,
                    commandLineOptions,
                    fileSystem,
                    console,
                    logger,
                    cancellationToken
                );

                if (result != 0)
                {
                    return result;
                }
            }

            commandLineFormatterResult.ElapsedMilliseconds = (long)
                Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds;
            if (!commandLineOptions.WriteStdout)
            {
                logger.LogInformation(
                    $"{(commandLineOptions.Check ? "Checked" : "Formatted")} {commandLineFormatterResult.Files} files in {commandLineFormatterResult.ElapsedMilliseconds}ms."
                );
            }

            return ReturnExitCode(commandLineOptions, commandLineFormatterResult);
        }
        catch (Exception ex)
            when (ex is InvalidIgnoreFileException
                || ex.InnerException is InvalidIgnoreFileException
            )
        {
            var invalidIgnoreFileException =
                ex is InvalidIgnoreFileException ? ex : ex.InnerException;

            logger.LogError(
                invalidIgnoreFileException!.InnerException,
                invalidIgnoreFileException.Message
            );
            return 1;
        }
    }

    private static async Task<int> FormatPhysicalFiles(
        CommandLineFormatterResult commandLineFormatterResult,
        CommandLineOptions commandLineOptions,
        IFileSystem fileSystem,
        IConsole console,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        IFormattedFileWriter? writer;
        if (commandLineOptions.WriteStdout)
        {
            writer = new StdOutFormattedFileWriter(console);
        }
        else if (commandLineOptions.Check || commandLineOptions.SkipWrite)
        {
            writer = new NullFormattedFileWriter();
        }
        else
        {
            writer = new FileSystemFormattedFileWriter(fileSystem);
        }

        for (var x = 0; x < commandLineOptions.DirectoryOrFilePaths.Length; x++)
        {
            var directoryOrFilePath = commandLineOptions.DirectoryOrFilePaths[x];
            var isFile = fileSystem.File.Exists(directoryOrFilePath);
            var isDirectory = fileSystem.Directory.Exists(directoryOrFilePath);

            if (!isFile && !isDirectory)
            {
                console.WriteErrorLine(
                    "There was no file or directory found at "
                        + commandLineOptions.OriginalDirectoryOrFilePaths[x]
                );
                return 1;
            }

            var directoryName = isFile
                ? fileSystem.Path.GetDirectoryName(directoryOrFilePath)
                : directoryOrFilePath;

            ArgumentNullException.ThrowIfNull(directoryName);

            var optionsProvider = await OptionsProvider.Create(
                directoryName,
                commandLineOptions.ConfigPath,
                commandLineOptions.IgnorePath,
                fileSystem,
                logger,
                cancellationToken
            );

            var originalDirectoryOrFile = commandLineOptions.OriginalDirectoryOrFilePaths[x];

            var formattingCache = await FormattingCacheFactory.InitializeAsync(
                commandLineOptions,
                optionsProvider,
                fileSystem,
                cancellationToken
            );

            if (!Path.IsPathRooted(originalDirectoryOrFile))
            {
                if (!originalDirectoryOrFile.StartsWith('.'))
                {
                    originalDirectoryOrFile =
                        "." + Path.DirectorySeparatorChar + originalDirectoryOrFile;
                }
            }

            async Task FormatFile(
                string actualFilePath,
                string originalFilePath,
                bool warnForUnsupported = false
            )
            {
                if (
                    (
                        !commandLineOptions.IncludeGenerated
                        && GeneratedCodeUtilities.IsGeneratedCodeFile(actualFilePath)
                    ) || await optionsProvider.IsIgnoredAsync(actualFilePath, cancellationToken)
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
                    await FormatPhysicalFile(
                        actualFilePath,
                        originalFilePath,
                        fileSystem,
                        logger,
                        commandLineFormatterResult,
                        writer,
                        commandLineOptions,
                        printerOptions,
                        formattingCache,
                        cancellationToken
                    );
                }
                else if (warnForUnsupported)
                {
                    var fileIssueLogger = new FileIssueLogger(
                        originalFilePath,
                        logger,
                        logFormat: LogFormat.Console
                    );
                    fileIssueLogger.WriteWarning("Is an unsupported file type.");
                }
            }

            if (isFile)
            {
                await FormatFile(directoryOrFilePath, originalDirectoryOrFile, true);
            }
            else if (isDirectory)
            {
                if (
                    !commandLineOptions.NoMSBuildCheck
                    && HasMismatchedCliAndMsBuildVersions.Check(
                        directoryOrFilePath,
                        fileSystem,
                        logger
                    )
                )
                {
                    return 1;
                }

                var tasks = fileSystem
                    .Directory.EnumerateFiles(
                        directoryOrFilePath,
                        "*.*",
                        SearchOption.AllDirectories
                    )
                    .Select(o =>
                        FormatFile(o, o.Replace(directoryOrFilePath, originalDirectoryOrFile))
                    )
                    .ToArray();
                try
                {
                    await Task.WhenAll(tasks).WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken != cancellationToken)
                    {
                        throw;
                    }
                }
            }

            await formattingCache.ResolveAsync(cancellationToken);
        }

        return 0;
    }

    private static async Task FormatPhysicalFile(
        string actualFilePath,
        string originalFilePath,
        IFileSystem fileSystem,
        ILogger logger,
        CommandLineFormatterResult commandLineFormatterResult,
        IFormattedFileWriter writer,
        CommandLineOptions commandLineOptions,
        PrinterOptions printerOptions,
        IFormattingCache formattingCache,
        CancellationToken cancellationToken
    )
    {
        var fileToFormatInfo = await FileToFormatInfo.CreateFromFileSystem(
            actualFilePath,
            fileSystem,
            cancellationToken
        );

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

        await PerformFormattingSteps(
            fileToFormatInfo,
            writer,
            commandLineFormatterResult,
            fileIssueLogger,
            printerOptions,
            commandLineOptions,
            formattingCache,
            cancellationToken
        );
    }

    private static int ReturnExitCode(
        CommandLineOptions commandLineOptions,
        CommandLineFormatterResult result
    )
    {
        if (
            (!commandLineOptions.CompilationErrorsAsWarnings && result.FailedCompilation > 0)
            || (commandLineOptions.Check && result.UnformattedFiles > 0)
            || result.FailedFormattingValidation > 0
            || result.ExceptionsFormatting > 0
            || result.ExceptionsValidatingSource > 0
        )
        {
            return 1;
        }

        return 0;
    }

    private static async Task PerformFormattingSteps(
        FileToFormatInfo fileToFormatInfo,
        IFormattedFileWriter formattedFileWriter,
        CommandLineFormatterResult commandLineFormatterResult,
        FileIssueLogger fileIssueLogger,
        PrinterOptions printerOptions,
        CommandLineOptions commandLineOptions,
        IFormattingCache formattingCache,
        CancellationToken cancellationToken
    )
    {
        if (fileToFormatInfo.FileContents.Length == 0)
        {
            return;
        }

        Interlocked.Increment(ref commandLineFormatterResult.Files);

        if (formattingCache.CanSkipFormatting(fileToFormatInfo))
        {
            Interlocked.Increment(ref commandLineFormatterResult.CachedFiles);
            return;
        }

        if (fileToFormatInfo.UnableToDetectEncoding)
        {
            fileIssueLogger.WriteWarning(
                $"Unable to detect file encoding. Defaulting to {fileToFormatInfo.Encoding}."
            );
        }

        cancellationToken.ThrowIfCancellationRequested();

        CodeFormatterResult? codeFormattingResult;

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
            Interlocked.Increment(ref commandLineFormatterResult.ExceptionsFormatting);
            return;
        }

        if (codeFormattingResult.CompilationErrors.Any())
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("Failed to compile so was not formatted.");
            foreach (var message in codeFormattingResult.CompilationErrors)
            {
                errorMessage.AppendLine(message.ToString());
            }

            if (!commandLineOptions.CompilationErrorsAsWarnings)
            {
                fileIssueLogger.WriteError(errorMessage.ToString());
            }
            else
            {
                fileIssueLogger.WriteWarning(errorMessage.ToString());
            }

            Interlocked.Increment(ref commandLineFormatterResult.FailedCompilation);
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
            IFormattingValidator? formattingValidator = null;

            if (printerOptions.Formatter is Formatter.CSharp or Formatter.CSharpScript)
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
                    var validatorResult = await formattingValidator.ValidateAsync(
                        cancellationToken
                    );
                    if (validatorResult.Failed)
                    {
                        Interlocked.Increment(
                            ref commandLineFormatterResult.FailedFormattingValidation
                        );
                        fileIssueLogger.WriteError(
                            $"Failed formatting validation.{(string.IsNullOrEmpty(validatorResult.FailureMessage) ? null : "/n" + validatorResult.FailureMessage)}"
                        );
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(
                        ref commandLineFormatterResult.ExceptionsValidatingSource
                    );

                    fileIssueLogger.WriteError(
                        "Failed with exception during syntax tree validation.",
                        ex
                    );
                }
            }
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
            fileIssueLogger.WriteError($"Was not formatted.\n{difference}\n");
            Interlocked.Increment(ref commandLineFormatterResult.UnformattedFiles);
        }

        formattedFileWriter.WriteResult(codeFormattingResult, fileToFormatInfo);
        formattingCache.CacheResult(codeFormattingResult.Code, fileToFormatInfo);
    }
}
