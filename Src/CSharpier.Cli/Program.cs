using System.CommandLine;
using CSharpier.Cli;

var rootCommand = new RootCommand();
rootCommand.AddCommand(FormattingCommands.CreateFormatCommand());
rootCommand.AddCommand(FormattingCommands.CreateCheckCommand());
rootCommand.AddCommand(PipeCommand.Create());
rootCommand.AddCommand(ServerCommand.Create());

return await rootCommand.InvokeAsync(args);
