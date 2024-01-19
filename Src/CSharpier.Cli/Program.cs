using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO.Abstractions;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CommandLineOptions.Create();

        rootCommand.Handler = CommandHandler.Create(new CommandLineOptions.Handler(Run));

        return await rootCommand.InvokeAsync(args);
    }

    // TODO at some point (1.0?) the options should be cleaned up
    // and use sub commands
    public static async Task<int> Run(
        string[]? directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool pipeMultipleFiles,
        bool ipc,
        int? ipcPort,
        bool noCache,
        bool noMSBuildCheck,
        bool includeGenerated,
        string configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        // System.CommandLine passes string.empty instead of null when this isn't supplied even if we use string?
        var actualConfigPath = string.IsNullOrEmpty(configPath) ? null : configPath;

        var console = new SystemConsole();
        var logger = new ConsoleLogger(console, logLevel);

        if (pipeMultipleFiles)
        {
            return await PipeMultipleFilesFormatter.StartServer(
                console,
                logger,
                actualConfigPath,
                cancellationToken
            );
        }

        if (ipc)
        {
            return await IpcFormatter.StartServer(
                ipcPort,
                logger,
                actualConfigPath,
                cancellationToken
            );
        }

        var directoryOrFileNotProvided = directoryOrFile is null or { Length: 0 };
        var originalDirectoryOrFile = directoryOrFile;

        string? standardInFileContents = null;
        if (Console.IsInputRedirected && directoryOrFileNotProvided)
        {
            using var streamReader = new StreamReader(
                Console.OpenStandardInput(),
                console.InputEncoding
            );
            standardInFileContents = await streamReader.ReadToEndAsync(
#if NET7_0_OR_GREATER
                cancellationToken
#endif
            );

            directoryOrFile = new[] { Directory.GetCurrentDirectory() };
            originalDirectoryOrFile = new[] { Directory.GetCurrentDirectory() };
        }
        else
        {
            directoryOrFile = directoryOrFile!
                .Select(
                    o =>
                        o == "."
                            // .csharpierignore gets confused by . so just don't include it
                            ? Directory.GetCurrentDirectory()
                            : Path.Combine(Directory.GetCurrentDirectory(), o)
                )
                .ToArray();
        }

        var commandLineOptions = new CommandLineOptions
        {
            DirectoryOrFilePaths = directoryOrFile.ToArray(),
            OriginalDirectoryOrFilePaths = originalDirectoryOrFile!,
            StandardInFileContents = standardInFileContents,
            Check = check,
            NoCache = noCache,
            NoMSBuildCheck = noMSBuildCheck,
            Fast = fast,
            SkipWrite = skipWrite,
            WriteStdout = writeStdout || standardInFileContents != null,
            IncludeGenerated = includeGenerated,
            ConfigPath = actualConfigPath
        };

        return await CommandLineFormatter.Format(
            commandLineOptions,
            new FileSystem(),
            console,
            logger,
            cancellationToken
        );
    }
}
