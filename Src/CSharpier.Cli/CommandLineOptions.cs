using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier.Cli;

public class CommandLineOptions
{
    public string[] DirectoryOrFilePaths { get; init; } = Array.Empty<string>();
    public bool Check { get; init; }
    public bool Fast { get; init; }
    public bool SkipWrite { get; init; }
    public bool WriteStdout { get; init; }
    public string? StandardInFileContents { get; init; }

    public bool ShouldWriteStandardOut => WriteStdout || StandardInFileContents != null;

    internal delegate Task<int> Handler(
        string[] directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
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
            )
        };

        rootCommand.AddValidator(
            cmd =>
            {
                if (!Console.IsInputRedirected && !cmd.Children.Contains("directoryOrFile"))
                {
                    return "directoryOrFile is required when not piping stdin to CSharpier";
                }

                return null;
            }
        );

        return rootCommand;
    }
}
