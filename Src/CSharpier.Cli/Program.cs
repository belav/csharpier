using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CommandLineOptions.Create();

        rootCommand.Handler = CommandHandler.Create(new CommandLineOptions.Handler(Run));

        return await rootCommand.InvokeAsync(args);
    }

    public static async Task<int> Run(
        string[]? directoryOrFile,
        bool check,
        bool fast,
        bool skipWrite,
        bool writeStdout,
        bool pipeMultipleFiles,
        CancellationToken cancellationToken
    )
    {
        DebugLogger.Log("Starting");
        var console = new SystemConsole();
        var logger = new ConsoleLogger(console);

        if (pipeMultipleFiles)
        {
            return await PipeMultipleFiles(console, logger, cancellationToken);
        }

        var directoryOrFileNotProvided = (directoryOrFile is null or { Length: 0 });
        var originalDirectoryOrFile = directoryOrFile;

        string? standardInFileContents = null;
        if (Console.IsInputRedirected && directoryOrFileNotProvided)
        {
            using var streamReader = new StreamReader(
                Console.OpenStandardInput(),
                console.InputEncoding
            );
            standardInFileContents = await streamReader.ReadToEndAsync();

            directoryOrFile = new[] { Directory.GetCurrentDirectory() };
            originalDirectoryOrFile = new[] { Directory.GetCurrentDirectory() };
        }
        else
        {
            directoryOrFile = directoryOrFile!
                .Select(
                    o =>
                        o == "."
                            // .csharpierignore gets confused by . so just don't include it
                            ? Directory.GetCurrentDirectory()
                            : Path.Combine(Directory.GetCurrentDirectory(), o)
                )
                .ToArray();
        }

        var commandLineOptions = new CommandLineOptions
        {
            DirectoryOrFilePaths = directoryOrFile!.ToArray(),
            OriginalDirectoryOrFilePaths = originalDirectoryOrFile!,
            StandardInFileContents = standardInFileContents,
            Check = check,
            Fast = fast,
            SkipWrite = skipWrite,
            WriteStdout = writeStdout || standardInFileContents != null
        };

        return await CommandLineFormatter.Format(
            commandLineOptions,
            new FileSystem(),
            console,
            logger,
            cancellationToken
        );
    }

    private static async Task<int> PipeMultipleFiles(
        SystemConsole console,
        ILogger logger,
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
                        Path.Combine(Directory.GetCurrentDirectory(), fileName)
                    },
                    OriginalDirectoryOrFilePaths = new[]
                    {
                        Path.IsPathRooted(fileName)
                          ? fileName
                          : fileName.StartsWith(".")
                              ? fileName
                              : "./" + fileName
                    },
                    StandardInFileContents = stringBuilder.ToString(),
                    Fast = true,
                    WriteStdout = true
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

                    console.Write('\u0003'.ToString());

                    if (result != 0)
                    {
                        exitCode = result;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed!");
                }

                stringBuilder.Clear();
                fileName = null;
            }
        }
    }
}
