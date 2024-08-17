namespace CSharpier.Cli;

using System.IO.Abstractions;
using System.Text;
using Microsoft.Extensions.Logging;

internal static class PipeMultipleFilesFormatter
{
    public static async Task<int> StartServer(
        SystemConsole console,
        ILogger logger,
        string? configPath,
        CancellationToken cancellationToken
    )
    {
        using var streamReader = new StreamReader(
            Console.OpenStandardInput(),
            console.InputEncoding
        );

        var stringBuilder = new StringBuilder();
        string? fileName = null;

        var exitCode = 0;

        while (true)
        {
            while (true)
            {
                var value = streamReader.Read();
                if (value == -1)
                {
                    return exitCode;
                }
                var character = Convert.ToChar(value);
                DebugLogger.Log("Got " + character);
                if (character == '\u0003')
                {
                    DebugLogger.Log("Got EOF");
                    break;
                }

                stringBuilder.Append(character);
            }

            if (fileName == null)
            {
                fileName = stringBuilder.ToString();
                stringBuilder.Clear();
            }
            else
            {
                var commandLineOptions = new CommandLineOptions
                {
                    DirectoryOrFilePaths = new[]
                    {
                        Path.Combine(Directory.GetCurrentDirectory(), fileName),
                    },
                    OriginalDirectoryOrFilePaths = new[]
                    {
                        Path.IsPathRooted(fileName) ? fileName
                        : fileName.StartsWith(".") ? fileName
                        : "./" + fileName,
                    },
                    StandardInFileContents = stringBuilder.ToString(),
                    Fast = true,
                    WriteStdout = true,
                    ConfigPath = configPath,
                };

                try
                {
                    var result = await CommandLineFormatter.Format(
                        commandLineOptions,
                        new FileSystem(),
                        console,
                        logger,
                        cancellationToken
                    );

                    if (result != 0)
                    {
                        exitCode = result;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed!");
                }
                finally
                {
                    console.Write('\u0003'.ToString());
                }

                stringBuilder.Clear();
                fileName = null;
            }
        }
    }
}
