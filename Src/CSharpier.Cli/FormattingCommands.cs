using System.CommandLine;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal static class FormattingCommands
{
    public static Command CreateFormatCommand()
    {
        var formatCommand = new Command("format", "Format files.");
        var directoryOrFileArgument = AddDirectoryOrFileArgument(formatCommand);

        var noCacheOption = new Option<bool>(
            ["--no-cache"],
            "Bypass the cache to determine if a file needs to be formatted."
        );
        formatCommand.AddOption(noCacheOption);
        formatCommand.AddOption(NoMsBuildCheckOption);
        formatCommand.AddOption(IncludeGeneratedOption);
        var fastOption = new Option<bool>(
            ["--fast"],
            "Skip comparing syntax tree of formatted file to original file to validate changes."
        );
        formatCommand.AddOption(fastOption);
        var skipWriteOption = new Option<bool>(
            ["--skip-write"],
            "Skip writing changes. Generally used for testing to ensure csharpier doesn't throw any errors or cause syntax tree validation failures."
        );
        formatCommand.AddOption(skipWriteOption);
        var writeStdoutOption = new Option<bool>(
            ["--write-stdout"],
            "Write the results of formatting any files to stdout."
        );
        formatCommand.AddOption(writeStdoutOption);
        formatCommand.AddOption(CompilationErrorsAsWarningsOption);
        formatCommand.AddOption(ConfigPathOption);
        formatCommand.AddOption(LogLevelOption);

        formatCommand.SetHandler(async context =>
        {
            var directoryOrFile = context.ParseResult.GetValueForArgument(directoryOrFileArgument);
            var fast = context.ParseResult.GetValueForOption(fastOption);
            var skipWrite = context.ParseResult.GetValueForOption(skipWriteOption);
            var writeStdout = context.ParseResult.GetValueForOption(writeStdoutOption);
            var noCache = context.ParseResult.GetValueForOption(noCacheOption);
            var noMSBuildCheck = context.ParseResult.GetValueForOption(NoMsBuildCheckOption);
            var includeGenerated = context.ParseResult.GetValueForOption(IncludeGeneratedOption);
            var compilationErrorsAsWarnings = context.ParseResult.GetValueForOption(
                CompilationErrorsAsWarningsOption
            );
            var configPath = context.ParseResult.GetValueForOption(ConfigPathOption);
            var logLevel = context.ParseResult.GetValueForOption(LogLevelOption);
            var token = context.GetCancellationToken();

            context.ExitCode = await FormatOrCheck(
                directoryOrFile,
                check: false,
                fast,
                skipWrite,
                writeStdout,
                noCache,
                noMSBuildCheck,
                includeGenerated,
                compilationErrorsAsWarnings,
                configPath,
                logLevel,
                token
            );
        });

        return formatCommand;
    }

    public static Command CreateCheckCommand()
    {
        var checkCommand = new Command(
            "check",
            "Check that files are formatted. Will not write any changes."
        );
        var directoryOrFileArgument = AddDirectoryOrFileArgument(checkCommand);

        checkCommand.AddOption(ConfigPathOption);
        checkCommand.AddOption(LogLevelOption);
        checkCommand.AddOption(IncludeGeneratedOption);
        checkCommand.AddOption(NoMsBuildCheckOption);
        checkCommand.AddOption(CompilationErrorsAsWarningsOption);

        checkCommand.SetHandler(async context =>
        {
            var directoryOrFile = context.ParseResult.GetValueForArgument(directoryOrFileArgument);
            var noMSBuildCheck = context.ParseResult.GetValueForOption(NoMsBuildCheckOption);
            var includeGenerated = context.ParseResult.GetValueForOption(IncludeGeneratedOption);
            var compilationErrorsAsWarnings = context.ParseResult.GetValueForOption(
                CompilationErrorsAsWarningsOption
            );
            var configPath = context.ParseResult.GetValueForOption(ConfigPathOption);
            var logLevel = context.ParseResult.GetValueForOption(LogLevelOption);
            var token = context.GetCancellationToken();

            context.ExitCode = await FormatOrCheck(
                directoryOrFile,
                check: true,
                fast: false,
                skipWrite: false,
                writeStdout: false,
                noCache: false,
                noMSBuildCheck,
                includeGenerated,
                compilationErrorsAsWarnings,
                configPath,
                logLevel,
                token
            );
        });

        return checkCommand;
    }

    private static async Task<int> FormatOrCheck(
        string[]? directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool noCache,
        bool noMSBuildCheck,
        bool includeGenerated,
        bool compilationErrorsAsWarnings,
        string? configPath,
        LogLevel logLevel,
        CancellationToken cancellationToken
    )
    {
        var console = new SystemConsole();
        var logger = new ConsoleLogger(console, logLevel);

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
            ConfigPath = configPath,
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

    private static Argument<string[]> AddDirectoryOrFileArgument(Command command)
    {
        var argument = new Argument<string[]>("directoryOrFile")
        {
            Arity = ArgumentArity.ZeroOrMore,
            Description =
                "One or more paths to a directory containing C# files to format or a C# file to format. It may be ommited when piping data via stdin.",
        }.LegalFilePathsOnly();
        command.AddArgument(argument);
        command.AddValidator(commandResult =>
        {
            if (!Console.IsInputRedirected && commandResult.FindResultFor(argument) is null)
            {
                commandResult.ErrorMessage =
                    "directoryOrFile is required when not piping stdin to CSharpier";
            }
        });

        return argument;
    }

    private static readonly Option<string> ConfigPathOption = new(
        ["--config-path"],
        "Path to the CSharpier configuration file"
    );

    private static readonly Option<LogLevel> LogLevelOption = new(
        ["--loglevel"],
        () => LogLevel.Information,
        "Specify the log level - Debug, Information (default), Warning, Error, None"
    );

    private static readonly Option<bool> IncludeGeneratedOption = new(
        ["--include-generated"],
        "Include files generated by the SDK and files that begin with <autogenerated /> comments"
    );

    private static readonly Option<bool> NoMsBuildCheckOption = new(
        ["--no-msbuild-check"],
        "Bypass the check to determine if a csproj files references a different version of CSharpier.MsBuild."
    );

    private static readonly Option<bool> CompilationErrorsAsWarningsOption = new(
        ["--compilation-errors-as-warnings"],
        "Treat compilation errors from files as warnings instead of errors."
    );
}
