using System.CommandLine;

namespace CSharpier.Cli;

public class CommandLineOptions
{
    public string[] DirectoryOrFilePaths { get; init; } = Array.Empty<string>();
    public bool Check { get; init; }
    public bool Fast { get; init; }
    public bool SkipWrite { get; init; }
    public bool WriteStdout { get; init; }
    public bool PipeMultipleFiles { get; init; }
    public bool NoCache { get; init; }
    public string? StandardInFileContents { get; init; }
    public string[] OriginalDirectoryOrFilePaths { get; init; } = Array.Empty<string>();

    internal delegate Task<int> Handler(
        string[] directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool pipeMultipleFiles,
        bool noCache,
        CancellationToken cancellationToken
    );

    public static RootCommand Create()
    {
        var rootCommand = new RootCommand
        {
            new Argument<string[]>("directoryOrFile")
            {
                Arity = ArgumentArity.ZeroOrMore,
                Description =
                    "One or more paths to a directory containing c# files to format or a c# file to format. If a path is not specified the current directory is used"
            }.LegalFilePathsOnly(),
            new Option(
                new[] { "--check" },
                "Check that files are formatted. Will not write any changes."
            ),
            new Option(
                new[] { "--no-cache" },
                "Bypass the cache to determine if a file needs to be formatted."
            ),
            new Option(
                new[] { "--fast" },
                "Skip comparing syntax tree of formatted file to original file to validate changes."
            ),
            new Option(
                new[] { "--skip-write" },
                "Skip writing changes. Generally used for testing to ensure csharpier doesn't throw any errors or cause syntax tree validation failures."
            ),
            new Option(
                new[] { "--write-stdout" },
                "Write the results of formatting any files to stdout."
            ),
            new Option(
                new[] { "--pipe-multiple-files" },
                "Keep csharpier running so that multiples files can be piped to it via stdin"
            )
        };

        rootCommand.AddValidator(cmd =>
        {
            if (!Console.IsInputRedirected && cmd.Children.Contains("--pipe-multiple-files"))
            {
                return "--pipe-multiple-files may only be used if you pipe stdin to CSharpier";
            }
            if (!Console.IsInputRedirected && !cmd.Children.Contains("directoryOrFile"))
            {
                return "directoryOrFile is required when not piping stdin to CSharpier";
            }

            return null;
        });

        return rootCommand;
    }
}
