using System.CommandLine;
using CSharpier.Cli;

var rootCommand = new RootCommand();
rootCommand.AddCommand(FormatingCommands.CreateFormatCommand());
rootCommand.AddCommand(FormatingCommands.CreateCheckCommand());
rootCommand.AddCommand(PipeCommand.Create());
rootCommand.AddCommand(ServerCommand.Create());

return await rootCommand.InvokeAsync(args);
