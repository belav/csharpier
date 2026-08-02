using System.Diagnostics;
using System.IO.Abstractions;
using CSharpier.Cli.Options;
using CSharpier.Core;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class CommandLineFormatter(
    CommandLineOptions commandLineOptions,
    IFileSystem fileSystem,
    IConsole console,
    ILogger logger,
    CancellationToken cancellationToken
)
{
    private readonly CommandLineFormatterResult result = new();

    public static Task<int> Format(
        CommandLineOptions commandLineOptions,
        IFileSystem fileSystem,
        IConsole console,
        ILogger logger,
        CancellationToken cancellationToken
    ) =>
        new CommandLineFormatter(
            commandLineOptions,
            fileSystem,
            console,
            logger,
            cancellationToken
        ).FormatAsync();

    private async Task<int> FormatAsync()
    {
        try
        {
            var timestamp = Stopwatch.GetTimestamp();

            if (commandLineOptions.StandardInFileContents != null)
            {
                await this.FormatStandardInput();
            }
            else
            {
                var exitCode = await this.FormatPhysicalFiles();
                if (exitCode != 0)
                {
                    return exitCode;
                }
            }

            this.result.ElapsedMilliseconds = (long)
                Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds;
            if (!commandLineOptions.WriteStdout)
            {
                logger.LogInformation(
                    $"{(commandLineOptions.Check ? "Checked" : "Formatted")} {this.result.Files} files in {this.result.ElapsedMilliseconds}ms."
                );
            }

            return this.ReturnExitCode();
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

    private async Task FormatStandardInput()
    {
        var standardInFileContents = commandLineOptions.StandardInFileContents;
        ArgumentNullException.ThrowIfNull(standardInFileContents);

        var pathSupplied = false;
        var directoryPath = commandLineOptions.DirectoryOrFilePaths[0];
        string? filePath;

        // when piping multiple files we get a path to a file here
        // when sending stdin-filepath we get a path to a file here
        // so because this path is not a directory one of those is true
        if (!fileSystem.Directory.Exists(directoryPath))
        {
            filePath = directoryPath;
            directoryPath = fileSystem.Path.GetDirectoryName(directoryPath);
            ArgumentNullException.ThrowIfNull(directoryPath);

            // The directory from --stdin-path may not exist on disk, but config
            // resolution enumerates files in it (CSharpierConfigParser) and throws
            // DirectoryNotFoundException when it is missing, so walk up to the
            // nearest existing ancestor. Stop at the filesystem root (GetDirectoryName
            // returns null) so a path with no existing ancestor cannot loop unbounded.
            while (!fileSystem.Directory.Exists(directoryPath))
            {
                var parentDirectory = fileSystem.Path.GetDirectoryName(directoryPath);
                if (parentDirectory is null)
                {
                    break;
                }

                directoryPath = parentDirectory;
            }

            pathSupplied = true;
        }
        // otherwise someone is running this as a single command and not sending a path
        else
        {
            filePath = fileSystem.Path.Combine(directoryPath, Guid.NewGuid().ToString());
            if (standardInFileContents.TrimStart().StartsWith('<'))
            {
                filePath += ".xml";
            }
            else
            {
                filePath += ".cs";
            }
        }

        var fileToFormatInfo = FileToFormatInfo.Create(
            filePath,
            standardInFileContents,
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

        var formattingEngine = new FormattingEngine(
            new StdOutFormattedFileWriter(console),
            optionsProvider,
            FormattingCacheFactory.NullCache,
            commandLineOptions,
            fileSystem,
            logger,
            this.result
        );

        await formattingEngine.FormatStandardInputFile(
            fileToFormatInfo,
            commandLineOptions.OriginalDirectoryOrFilePaths[0],
            checkIsIgnored: pathSupplied,
            cancellationToken
        );
    }

    private async Task<int> FormatPhysicalFiles()
    {
        var writer = this.SelectWriter();

        for (var x = 0; x < commandLineOptions.DirectoryOrFilePaths.Length; x++)
        {
            var exitCode = await this.FormatPhysicalPath(x, writer);
            if (exitCode != 0)
            {
                return exitCode;
            }
        }

        return 0;
    }

    private IFormattedFileWriter SelectWriter()
    {
        if (commandLineOptions.WriteStdout)
        {
            return new StdOutFormattedFileWriter(console);
        }

        if (commandLineOptions.Check || commandLineOptions.SkipWrite)
        {
            return new NullFormattedFileWriter();
        }

        return new FileSystemFormattedFileWriter(fileSystem);
    }

    private async Task<int> FormatPhysicalPath(int index, IFormattedFileWriter writer)
    {
        var directoryOrFilePath = commandLineOptions.DirectoryOrFilePaths[index];
        var isFile = fileSystem.File.Exists(directoryOrFilePath);
        var isDirectory = fileSystem.Directory.Exists(directoryOrFilePath);

        if (!isFile && !isDirectory)
        {
            console.WriteErrorLine(
                "There was no file or directory found at "
                    + commandLineOptions.OriginalDirectoryOrFilePaths[index]
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

        var originalDirectoryOrFile = commandLineOptions.OriginalDirectoryOrFilePaths[index];

        var formattingCache = await FormattingCacheFactory.InitializeAsync(
            commandLineOptions,
            optionsProvider,
            fileSystem,
            cancellationToken
        );

        if (!fileSystem.Path.IsPathRooted(originalDirectoryOrFile))
        {
            if (!originalDirectoryOrFile.StartsWith('.'))
            {
                originalDirectoryOrFile =
                    "." + fileSystem.Path.DirectorySeparatorChar + originalDirectoryOrFile;
            }
        }

        var formattingEngine = new FormattingEngine(
            writer,
            optionsProvider,
            formattingCache,
            commandLineOptions,
            fileSystem,
            logger,
            this.result
        );

        if (isFile)
        {
            await formattingEngine.FormatPhysicalFile(
                directoryOrFilePath,
                originalDirectoryOrFile,
                warnForUnsupported: true,
                cancellationToken
            );
        }
        else if (isDirectory)
        {
            if (
                !commandLineOptions.NoMSBuildCheck
                && await HasMismatchedCliAndMsBuildVersions.Check(
                    directoryOrFilePath,
                    fileSystem,
                    logger,
                    cancellationToken
                )
            )
            {
                return 1;
            }

            await formattingEngine.FormatDirectory(
                directoryOrFilePath,
                originalDirectoryOrFile,
                cancellationToken
            );
        }

        await formattingCache.ResolveAsync(cancellationToken);

        return 0;
    }

    private int ReturnExitCode()
    {
        if (
            (!commandLineOptions.SyntaxErrorsAsWarnings && this.result.FailedCompilation > 0)
            || (
                commandLineOptions.Check
                && this.result.UnformattedFiles > 0
                && !commandLineOptions.UnformattedAsWarnings
            )
            || this.result.FailedFormattingValidation > 0
            || this.result.ExceptionsFormatting > 0
            || this.result.ExceptionsValidatingSource > 0
        )
        {
            return 1;
        }

        return 0;
    }
}
