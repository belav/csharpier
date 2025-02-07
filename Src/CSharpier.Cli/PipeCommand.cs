using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal static class PipeCommand
{
    public static Command Create()
    {
        var pipeMultipleFilesCommand = new Command(
            "pipe-files",
            "Keep csharpier running so that multiples files can be piped to it via stdin."
        );

        pipeMultipleFilesCommand.AddValidator(result =>
        {
            if (!Console.IsInputRedirected)
            {
                result.ErrorMessage = "pipe-files may only be used if you pipe stdin to CSharpier";
            }
        });

        pipeMultipleFilesCommand.SetHandler(async context =>
        {
            var logLevel = LogLevel.Information;

            var console = new SystemConsole();
            var logger = new ConsoleLogger(console, logLevel);

            var cancellationToken = context.GetCancellationToken();
            context.ExitCode = await PipeMultipleFilesFormatter.StartServer(
                console,
                logger,
                cancellationToken
            );
        });

        return pipeMultipleFilesCommand;
    }
}
