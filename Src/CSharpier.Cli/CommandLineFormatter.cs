using System.Diagnostics;
using System.IO.Abstractions;
using CSharpier.Utilities;
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
            var stopwatch = Stopwatch.StartNew();
            var commandLineFormatterResult = new CommandLineFormatterResult();

            // TODO maybe the IFileInfo, IFormattedFileWriter and these two checks should just go back to the way they were
            // sending through IFileSystem and CommandLineOptions is probably less parameters.
            var performSyntaxTreeValidation = !commandLineOptions.Fast;
            var performCheck = commandLineOptions.Check && !commandLineOptions.WriteStdout;

            if (commandLineOptions.StandardInFileContents != null)
            {
                var filePath = commandLineOptions.DirectoryOrFilePaths[0];
                var provider = new StandardInFileInfo(
                    commandLineOptions.StandardInFileContents,
                    console.InputEncoding
                );

                var loggerAndOptions = await GetLoggerAndOptions(
                    filePath,
                    filePath,
                    fileSystem,
                    logger,
                    cancellationToken
                );

                if (loggerAndOptions != null)
                {
                    await PerformFormattingSteps(
                        provider,
                        new StdOutFormattedFileWriter(console),
                        commandLineFormatterResult,
                        loggerAndOptions.Value.fileIssueLogger,
                        loggerAndOptions.Value.printerOptions,
                        performSyntaxTreeValidation,
                        performCheck,
                        cancellationToken
                    );
                }
            }
            else
            {
                IFormattedFileWriter? writer = null;
                if (commandLineOptions.WriteStdout)
                {
                    writer = new StdOutFormattedFileWriter(console);
                }
                else if (commandLineOptions.Check || commandLineOptions.SkipWrite)
                {
                    writer = new NullFormattedFileWriter();
                }

                foreach (var directoryOrFile in commandLineOptions.DirectoryOrFilePaths)
                {
                    async Task FormatFile(string filePath)
                    {
                        await FormatPhysicalFile(
                            filePath,
                            directoryOrFile,
                            fileSystem,
                            logger,
                            commandLineFormatterResult,
                            writer,
                            performSyntaxTreeValidation,
                            performCheck,
                            cancellationToken
                        );
                    }

                    if (fileSystem.File.Exists(directoryOrFile))
                    {
                        await FormatFile(directoryOrFile);
                    }
                    else
                    {
                        var tasks = fileSystem.Directory
                            .EnumerateFiles(directoryOrFile, "*.cs", SearchOption.AllDirectories)
                            .Select(FormatFile)
                            .ToArray();
                        try
                        {
                            Task.WaitAll(tasks, cancellationToken);
                        }
                        catch (OperationCanceledException ex)
                        {
                            if (ex.CancellationToken != cancellationToken)
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            commandLineFormatterResult.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            if (!commandLineOptions.WriteStdout)
            {
                ResultPrinter.PrintResults(commandLineFormatterResult, logger, commandLineOptions);
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
                invalidIgnoreFileException.InnerException,
                invalidIgnoreFileException.Message
            );
            return 1;
        }
    }

    private static async Task FormatPhysicalFile(
        string filePath,
        string directoryOrFile,
        IFileSystem fileSystem,
        ILogger logger,
        CommandLineFormatterResult commandLineFormatterResult,
        IFormattedFileWriter? writer,
        bool performSyntaxTreeValidation,
        bool performCheck,
        CancellationToken cancellationToken
    )
    {
        var provider = await PhysicalFileInfoAndWriter.Create(
            filePath,
            fileSystem,
            cancellationToken
        );

        var loggerAndOptions = await GetLoggerAndOptions(
            directoryOrFile,
            filePath,
            fileSystem,
            logger,
            cancellationToken
        );

        if (loggerAndOptions == null)
        {
            return;
        }

        if (!filePath.EndsWithIgnoreCase(".cs") && !filePath.EndsWithIgnoreCase(".cst"))
        {
            loggerAndOptions.Value.fileIssueLogger.WriteError("Is an unsupported file type.");
            return;
        }

        await PerformFormattingSteps(
            provider,
            writer ?? provider,
            commandLineFormatterResult,
            loggerAndOptions.Value.fileIssueLogger,
            loggerAndOptions.Value.printerOptions,
            performSyntaxTreeValidation,
            performCheck,
            cancellationToken
        );
    }

    private static async Task<(FileIssueLogger fileIssueLogger, PrinterOptions printerOptions)?> GetLoggerAndOptions(
        string pathToDirectoryOrFile,
        string pathToFile,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        var normalizedPath = pathToDirectoryOrFile.Replace('\\', '/');
        var baseDirectoryPath = fileSystem.Directory.Exists(normalizedPath)
            ? normalizedPath
            : fileSystem.Path.GetDirectoryName(normalizedPath);

        if (baseDirectoryPath == null)
        {
            throw new Exception(
                $"The path of {normalizedPath} does not appear to point to a directory or a file."
            );
        }

        var printerOptions = ConfigurationFileOptions.CreatePrinterOptions(
            baseDirectoryPath,
            fileSystem,
            logger
        );

        var ignoreFile = await IgnoreFile.Create(
            baseDirectoryPath,
            fileSystem,
            logger,
            cancellationToken
        );

        if (
            GeneratedCodeUtilities.IsGeneratedCodeFile(pathToFile)
            || ignoreFile.IsIgnored(pathToFile)
        )
        {
            return null;
        }

        var filePathLogger = new FileIssueLogger(pathToFile[baseDirectoryPath.Length..], logger);

        return (filePathLogger, printerOptions);
    }

    private static int ReturnExitCode(
        CommandLineOptions commandLineOptions,
        CommandLineFormatterResult result
    )
    {
        if (
            (commandLineOptions.StandardInFileContents != null && result.FailedCompilation > 0)
            || (commandLineOptions.Check && result.UnformattedFiles > 0)
            || result.FailedSyntaxTreeValidation > 0
            || result.ExceptionsFormatting > 0
            || result.ExceptionsValidatingSource > 0
        )
        {
            return 1;
        }

        return 0;
    }

    private static async Task PerformFormattingSteps(
        IFileInfo fileInfo,
        IFormattedFileWriter formattedFileWriter,
        CommandLineFormatterResult commandLineFormatterResult,
        FileIssueLogger fileIssueLogger,
        PrinterOptions printerOptions,
        bool performSyntaxTreeValidation,
        bool performCheck,
        CancellationToken cancellationToken
    )
    {
        if (fileInfo.FileContents.Length == 0)
        {
            return;
        }

        if (fileInfo.UnableToDetectEncoding)
        {
            fileIssueLogger.WriteWarning(
                $"Unable to detect file encoding. Defaulting to {fileInfo.Encoding}."
            );
        }

        cancellationToken.ThrowIfCancellationRequested();

        CodeFormatterResult codeFormattingResult;

        try
        {
            codeFormattingResult = await CodeFormatter.FormatAsync(
                fileInfo.FileContents,
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
        finally
        {
            Interlocked.Increment(ref commandLineFormatterResult.Files);
        }

        if (codeFormattingResult.Errors.Any())
        {
            fileIssueLogger.WriteError("Failed to compile so was not formatted.");
            Interlocked.Increment(ref commandLineFormatterResult.FailedCompilation);
            return;
        }

        if (!codeFormattingResult.FailureMessage.IsBlank())
        {
            fileIssueLogger.WriteError(codeFormattingResult.FailureMessage);
            return;
        }

        if (performSyntaxTreeValidation)
        {
            var syntaxNodeComparer = new SyntaxNodeComparer(
                fileInfo.FileContents,
                codeFormattingResult.Code,
                cancellationToken
            );

            try
            {
                var failure = await syntaxNodeComparer.CompareSourceAsync(cancellationToken);
                if (!string.IsNullOrEmpty(failure))
                {
                    Interlocked.Increment(
                        ref commandLineFormatterResult.FailedSyntaxTreeValidation
                    );
                    fileIssueLogger.WriteError($"Failed syntax tree validation.\n{failure}");
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref commandLineFormatterResult.ExceptionsValidatingSource);

                fileIssueLogger.WriteError(
                    "Failed with exception during syntax tree validation.",
                    ex
                );
            }
        }

        if (performCheck && codeFormattingResult.Code != fileInfo.FileContents)
        {
            var difference = StringDiffer.PrintFirstDifference(
                codeFormattingResult.Code,
                fileInfo.FileContents
            );
            fileIssueLogger.WriteWarning($"Was not formatted.\n{difference}");
            Interlocked.Increment(ref commandLineFormatterResult.UnformattedFiles);
        }

        formattedFileWriter.WriteResult(codeFormattingResult);
    }
}
