using System.CommandLine;
using CSharpier.Cli.Server;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal static class ServerCommand
{
    public static Command Create()
    {
        var serverCommand = new Command(
            "server",
            "Run CSharpier as a server so that multiple files may be formatted."
        );
        var serverPortOption = new Option<int?>(
            ["--server-port"],
            "Specify the port that CSharpier should start on. Defaults to a random unused port."
        );
        serverCommand.AddOption(serverPortOption);

        serverCommand.SetHandler(async context =>
        {
            // TODO do these options even do anything??
            var logLevel = LogLevel.Information;

            var serverPort = context.ParseResult.GetValueForOption(serverPortOption);
            context.ExitCode = await ServerFormatter.StartServer(
                serverPort,
                new ConsoleLogger(new SystemConsole(), logLevel)
            );
        });

        return serverCommand;
    }
}
