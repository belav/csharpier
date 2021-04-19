using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace CSharpier
{
    class Program
    {
        // TODO 1 Configuration.cs from data.entities
        // TODO 1 CurrencyDto.cs from data.entities

        // TODO https://github.com/dotnet/command-line-api/blob/main/docs/Your-first-app-with-System-CommandLine-DragonFruit.md
        // can be used to simplify this, but it didn't appear to work with descriptions of the parameters
        static async Task<int> Main(string[] args)
        {
            var rootCommand = CommandLineOptions.Create();

            rootCommand.Handler = CommandHandler.Create(
                new CommandLineOptions.Handler(CommandLineFormatter.Run)
            );

            return await rootCommand.InvokeAsync(args);
        }
    }
}
