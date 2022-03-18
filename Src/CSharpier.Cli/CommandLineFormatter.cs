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
            async Task<(IgnoreFile, PrinterOptions)> GetIgnoreFileAndPrinterOptions(
                string directoryOrFile
            )
            {
                var isDirectory = fileSystem.Directory.Exists(directoryOrFile);

                var baseDirectoryPath = isDirectory
                    ? directoryOrFile
                    : fileSystem.Path.GetDirectoryName(directoryOrFile);

                var ignoreFile = await IgnoreFile.Create(
                    baseDirectoryPath,
                    fileSystem,
                    logger,
                    cancellationToken
                );

                var printerOptions = ConfigurationFileOptions.CreatePrinterOptions(
                    baseDirectoryPath,
                    fileSystem,
                    logger
                );

                return (ignoreFile, printerOptions);
            }

            var stopwatch = Stopwatch.StartNew();
            var commandLineFormatterResult = new CommandLineFormatterResult();

            if (commandLineOptions.StandardInFileContents != null)
            {
                var filePath = commandLineOptions.DirectoryOrFilePaths[0];
                var fileToFormatInfo = FileToFormatInfo.Create(
                    filePath,
                    commandLineOptions.StandardInFileContents,
                    console.InputEncoding
                );

                var (ignoreFile, printerOptions) = await GetIgnoreFileAndPrinterOptions(filePath);

                if (
                    !GeneratedCodeUtilities.IsGeneratedCodeFile(filePath)
                    && !ignoreFile.IsIgnored(filePath)
                )
                {
                    var fileIssueLogger = GetFileIssueLogger(
                        filePath,
                        filePath,
                        fileSystem,
                        logger
                    );

                    await PerformFormattingSteps(
                        fileToFormatInfo,
                        new StdOutFormattedFileWriter(console),
                        commandLineFormatterResult,
                        fileIssueLogger,
                        printerOptions,
                        commandLineOptions,
                        cancellationToken
                    );
                }
            }
            else
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

                foreach (
                    var directoryOrFile in commandLineOptions.DirectoryOrFilePaths.Select(
                        o => o.Replace("\\", "/")
                    )
                )
                {
                    var (ignoreFile, printerOptions) = await GetIgnoreFileAndPrinterOptions(
                        directoryOrFile
                    );

                    async Task FormatFile(string filePath)
                    {
                        if (
                            GeneratedCodeUtilities.IsGeneratedCodeFile(filePath)
                            || ignoreFile.IsIgnored(filePath)
                        )
                        {
                            return;
                        }

                        await FormatPhysicalFile(
                            filePath,
                            directoryOrFile,
                            fileSystem,
                            logger,
                            commandLineFormatterResult,
                            writer,
                            commandLineOptions,
                            printerOptions,
                            cancellationToken
                        );
                    }

                    if (fileSystem.File.Exists(directoryOrFile))
                    {
                        await FormatFile(directoryOrFile);
                    }
                    else if (fileSystem.Directory.Exists(directoryOrFile))
                    {
                        if (
                            HasMismatchedCliAndMsBuildVersions.Check(
                                directoryOrFile,
                                fileSystem,
                                logger
                            )
                        )
                        {
                            return 1;
                        }

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
                    else
                    {
                        console.WriteErrorLine(
                            "There was no file or directory found at " + directoryOrFile
                        );
                        return 1;
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
                invalidIgnoreFileException!.InnerException,
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
        IFormattedFileWriter writer,
        CommandLineOptions commandLineOptions,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        var fileToFormatInfo = await FileToFormatInfo.CreateFromFileSystem(
            filePath,
            fileSystem,
            cancellationToken
        );

        var fileIssueLogger = GetFileIssueLogger(directoryOrFile, filePath, fileSystem, logger);

        if (!filePath.EndsWithIgnoreCase(".cs") && !filePath.EndsWithIgnoreCase(".cst"))
        {
            fileIssueLogger.WriteError("Is an unsupported file type.");
            return;
        }

        await PerformFormattingSteps(
            fileToFormatInfo,
            writer,
            commandLineFormatterResult,
            fileIssueLogger,
            printerOptions,
            commandLineOptions,
            cancellationToken
        );
    }

    private static FileIssueLogger GetFileIssueLogger(
        string pathToDirectoryOrFile,
        string pathToFile,
        IFileSystem fileSystem,
        ILogger logger
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

        var filePathLogger = new FileIssueLogger(
            pathToFile.Replace('\\', '/')[baseDirectoryPath.Length..],
            logger
        );

        return filePathLogger;
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
        FileToFormatInfo fileToFormatInfo,
        IFormattedFileWriter formattedFileWriter,
        CommandLineFormatterResult commandLineFormatterResult,
        FileIssueLogger fileIssueLogger,
        PrinterOptions printerOptions,
        CommandLineOptions commandLineOptions,
        CancellationToken cancellationToken
    )
    {
        if (fileToFormatInfo.FileContents.Length == 0)
        {
            return;
        }

        if (fileToFormatInfo.UnableToDetectEncoding)
        {
            fileIssueLogger.WriteWarning(
                $"Unable to detect file encoding. Defaulting to {fileToFormatInfo.Encoding}."
            );
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

        if (!commandLineOptions.Fast)
        {
            var syntaxNodeComparer = new SyntaxNodeComparer(
                fileToFormatInfo.FileContents,
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

        if (
            commandLineOptions.Check
            && !commandLineOptions.WriteStdout
            && codeFormattingResult.Code != fileToFormatInfo.FileContents
        )
        {
            var difference = StringDiffer.PrintFirstDifference(
                codeFormattingResult.Code,
                fileToFormatInfo.FileContents
            );
            fileIssueLogger.WriteWarning($"Was not formatted.\n{difference}");
            Interlocked.Increment(ref commandLineFormatterResult.UnformattedFiles);
        }

        formattedFileWriter.WriteResult(codeFormattingResult, fileToFormatInfo);
    }
}
