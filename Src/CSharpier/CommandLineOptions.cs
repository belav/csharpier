using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    public class CommandLineOptions
    {
        public string[] DirectoryOrFilePaths { get; init; } = Array.Empty<string>();
        public bool Check { get; init; }
        public bool Fast { get; init; }
        public bool SkipWrite { get; init; }

        internal delegate Task<int> Handler(
            string[] directoryOrFile,
            bool check,
            bool fast,
            bool skipWrite,
            CancellationToken cancellationToken
        );

        public static RootCommand Create()
        {
            var rootCommand = new RootCommand
            {
                new Argument<string[]>(
                    "directoryOrFile"
                )
                {
                    Arity = ArgumentArity.ZeroOrMore,
                    Description = "One or more paths to a directory containing files to format or a file to format. If a path is not specified the current directory is used"
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
                )
            };

            rootCommand.Description = "csharpier";

            return rootCommand;
        }
    }
}
