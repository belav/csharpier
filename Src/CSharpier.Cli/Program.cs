using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO.Abstractions;
using System.Text;
using CSharpier.Cli.Server;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CommandLineOptions.Create();
        var formatCommand = CommandLineOptions.CreateFormatCommand();
        var checkCommand = CommandLineOptions.CreateCheckCommand();
        var pipeCommand = CommandLineOptions.CreatePipeCommand();
        var serverCommand = CommandLineOptions.CreateServerCommand();

        rootCommand.AddCommand(formatCommand);
        formatCommand.Handler = CommandHandler.Create(
            new CommandLineOptions.FormatHandler(RunFormat)
        );

        rootCommand.AddCommand(checkCommand);
        checkCommand.Handler = CommandHandler.Create(new CommandLineOptions.CheckHandler(RunCheck));

        rootCommand.AddCommand(pipeCommand);
        pipeCommand.Handler = CommandHandler.Create(new CommandLineOptions.PipeHandler(RunPipe));

        rootCommand.AddCommand(serverCommand);
        serverCommand.Handler = CommandHandler.Create(
            new CommandLineOptions.ServerHandler(RunServer)
        );

        return await rootCommand.InvokeAsync(args);
    }

    public static Task<int> RunFormat(
        string[]? directoryOrFile,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool noCache,
        bool noMSBuildCheck,
        bool includeGenerated,
        bool compilationErrorsAsWarnings,
        string configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        return Run(
            directoryOrFile,
            false,
            fast,
            skipWrite,
            writeStdout,
            noCache,
            noMSBuildCheck,
            includeGenerated,
            compilationErrorsAsWarnings,
            configPath,
            logLevel,
            cancellationToken
        );
    }

    public static Task<int> RunCheck(
        string[]? directoryOrFile,
        string configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        return Run(
            directoryOrFile,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            configPath,
            logLevel,
            cancellationToken
        );
    }

    public static async Task<int> RunPipe(
        string configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        var (actualConfigPath, console, logger) = CreateConsoleLoggerAndActualPath(
            configPath,
            logLevel
        );

        return await PipeMultipleFilesFormatter.StartServer(
            console,
            logger,
            actualConfigPath,
            cancellationToken
        );
    }

    public static async Task<int> RunServer(
        string configPath,
        int? serverPort,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        var (actualConfigPath, console, logger) = CreateConsoleLoggerAndActualPath(
            configPath,
            logLevel
        );

        return await ServerFormatter.StartServer(serverPort, logger, actualConfigPath);
    }

    public static async Task<int> Run(
        string[]? directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool noCache,
        bool noMSBuildCheck,
        bool includeGenerated,
        bool compilationErrorsAsWarnings,
        string configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        var (actualConfigPath, console, logger) = CreateConsoleLoggerAndActualPath(
            configPath,
            logLevel
        );

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

            directoryOrFile = [Directory.GetCurrentDirectory()];
            originalDirectoryOrFile = [Directory.GetCurrentDirectory()];
        }
        else
        {
            directoryOrFile = directoryOrFile!.Select(Path.GetFullPath).ToArray();
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
            ConfigPath = actualConfigPath,
            CompilationErrorsAsWarnings = compilationErrorsAsWarnings,
        };

        return await CommandLineFormatter.Format(
            commandLineOptions,
            new FileSystem(),
            console,
            logger,
            cancellationToken
        );
    }

    private static (
        string? actualConfigPath,
        SystemConsole console,
        ConsoleLogger logger
    ) CreateConsoleLoggerAndActualPath(string configPath, LogLevel logLevel)
    {
        // System.CommandLine passes string.empty instead of null when this isn't supplied even if we use string?
        var actualConfigPath = string.IsNullOrEmpty(configPath) ? null : configPath;

        var console = new SystemConsole();
        var logger = new ConsoleLogger(console, logLevel);
        return (actualConfigPath, console, logger);
    }
}
