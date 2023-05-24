using System.Diagnostics;
using System.IO.Abstractions;
using CSharpier.Utilities;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

using System.Text;

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

            if (commandLineOptions.StandardInFileContents != null)
            {
                var filePath = commandLineOptions.DirectoryOrFilePaths[0];
                var fileToFormatInfo = FileToFormatInfo.Create(
                    filePath,
                    commandLineOptions.StandardInFileContents,
                    console.InputEncoding
                );

                var (ignoreFile, printerOptions) = await GetIgnoreFileAndPrinterOptions(
                    filePath,
                    commandLineOptions.ConfigPath,
                    fileSystem,
                    logger,
                    cancellationToken
                );

                if (
                    !GeneratedCodeUtilities.IsGeneratedCodeFile(filePath)
                    && !ignoreFile.IsIgnored(filePath)
                )
                {
                    var fileIssueLogger = new FileIssueLogger(
                        commandLineOptions.OriginalDirectoryOrFilePaths[0],
                        logger
                    );

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
            var directoryOrFile = commandLineOptions.DirectoryOrFilePaths[x].Replace("\\", "/");
            var originalDirectoryOrFile = commandLineOptions.OriginalDirectoryOrFilePaths[
                x
            ].Replace("\\", "/");

            var (ignoreFile, printerOptions) = await GetIgnoreFileAndPrinterOptions(
                directoryOrFile,
                commandLineOptions.ConfigPath,
                fileSystem,
                logger,
                cancellationToken
            );

            var formattingCache = await FormattingCacheFactory.InitializeAsync(
                commandLineOptions,
                printerOptions,
                fileSystem,
                cancellationToken
            );

            if (!Path.IsPathRooted(originalDirectoryOrFile))
            {
                if (!originalDirectoryOrFile.StartsWith("."))
                {
                    originalDirectoryOrFile = "./" + originalDirectoryOrFile;
                }
            }

            async Task FormatFile(string actualFilePath, string originalFilePath)
            {
                if (
                    GeneratedCodeUtilities.IsGeneratedCodeFile(actualFilePath)
                    || ignoreFile.IsIgnored(actualFilePath)
                )
                {
                    return;
                }

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

            if (fileSystem.File.Exists(directoryOrFile))
            {
                await FormatFile(directoryOrFile, originalDirectoryOrFile);
            }
            else if (fileSystem.Directory.Exists(directoryOrFile))
            {
                if (
                    !commandLineOptions.NoMSBuildCheck
                    && HasMismatchedCliAndMsBuildVersions.Check(directoryOrFile, fileSystem, logger)
                )
                {
                    return 1;
                }

                var tasks = fileSystem.Directory
                    .EnumerateFiles(directoryOrFile, "*.cs", SearchOption.AllDirectories)
                    .Select(o =>
                    {
                        var normalizedPath = o.Replace("\\", "/");
                        return FormatFile(
                            normalizedPath,
                            normalizedPath.Replace(directoryOrFile, originalDirectoryOrFile)
                        );
                    })
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

            await formattingCache.ResolveAsync(cancellationToken);
        }

        return 0;
    }

    private static async Task<(IgnoreFile, PrinterOptions)> GetIgnoreFileAndPrinterOptions(
        string directoryOrFile,
        string? configPath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
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

        var printerOptions = configPath is null
            ? ConfigurationFileOptions.FindPrinterOptionsForDirectory(
                baseDirectoryPath,
                fileSystem,
                logger
            )
            : ConfigurationFileOptions.CreatePrinterOptionsFromPath(configPath, fileSystem, logger);

        return (ignoreFile, printerOptions);
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

        var fileIssueLogger = new FileIssueLogger(originalFilePath, logger);

        if (!actualFilePath.EndsWithIgnoreCase(".cs") && !actualFilePath.EndsWithIgnoreCase(".cst"))
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

        CodeFormatterResult codeFormattingResult;

        try
        {
            // TODO xml find correct formatter
            codeFormattingResult = await CSharpFormatter.FormatAsync(
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
            fileIssueLogger.WriteError(errorMessage.ToString());
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
            commandLineOptions is { Check: true, WriteStdout: false }
            && codeFormattingResult.Code != fileToFormatInfo.FileContents
        )
        {
            var difference = StringDiffer.PrintFirstDifference(
                codeFormattingResult.Code,
                fileToFormatInfo.FileContents
            );
            fileIssueLogger.WriteWarning($"Was not formatted.\n{difference}\n");
            Interlocked.Increment(ref commandLineFormatterResult.UnformattedFiles);
        }

        formattedFileWriter.WriteResult(codeFormattingResult, fileToFormatInfo);
        formattingCache.CacheResult(codeFormattingResult.Code, fileToFormatInfo);
    }
}
