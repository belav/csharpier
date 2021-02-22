using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace CSharpier.CLI
{
    public static class CommandLineOptions
    {
        internal delegate Task<int> Handler(
            string directory,
            bool validate);
        
        public static RootCommand Create()
        {
            var rootCommand = new RootCommand
            {
                new Argument<string>("directory")
                {
                    Arity = ArgumentArity.ZeroOrOne,
                    Description = "A path to a directory containing files to format. If a path is not specified the current directory is used"
                }.LegalFilePathsOnly(),
                new Option(new[] { "--validate", "-v" }, "Compare syntax tree of formatted file to original file to validate changes.")
            };

            rootCommand.Description = "csharpier";

            return rootCommand;
        }
    }
}