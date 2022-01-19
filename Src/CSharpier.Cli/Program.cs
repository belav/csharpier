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
        Log("Starting");
        var console = new SystemConsole();
        var logger = new ConsoleLogger(console);

        if (pipeMultipleFiles)
        {
            // TODO whatever I do to fix this, I need to also fix it for multiple file mode
            return await PipeMultipleFiles(console, logger, cancellationToken);
        }

        var directoryOrFileNotProvided = (directoryOrFile is null or { Length: 0 });

        string? standardInFileContents = null;
        if (Console.IsInputRedirected && directoryOrFileNotProvided)
        {
            using var streamReader = new StreamReader(
                Console.OpenStandardInput(),
                console.InputEncoding
            );
            standardInFileContents = await streamReader.ReadToEndAsync();

            /*
             Some of my findings so far, because this may not be super quick to resolve like I hoped.

- Both rider and VS format the file with ?
- VSCode gets a fails to compile error
- The problem is most likely on the csharpier side, I have reproduced it in a CLI test that pipes input to csharpier. I believe this is the closest thing to how the extensions work.
- When I use windows terminal to pipe to csharpier, I get a fail to compile error about too many characters in literal
- When I use the terminal in rider the character turns into a ? when it is pasted
- I haven't tried it in the regular windows command prompt
- Program.cs reads the input from the console, and I think it is reading it incorrectly. Or I am logging out what I get from it incorrectly.

             */

            File.AppendAllText("c:/temp/log.txt", "Input:");
            File.AppendAllText("c:/temp/log.txt", standardInFileContents);
            File.AppendAllText("c:/temp/log.txt", "\n");

            directoryOrFile = new[] { Directory.GetCurrentDirectory() };
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

    // this is used for troubleshooting new IDE plugins and can eventually go away.
    [Conditional("DEBUG")]
    private static void Log(string message)
    {
        try
        {
            File.AppendAllText(
                @"C:\projects\csharpier\Src\CSharpier.Cli\bin\Debug\net6.0\log.txt",
                message + "\n"
            );
        }
        catch (Exception)
        {
            // we don't care if this fails
        }
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
                Log("Got " + character);
                if (character == '\u0003')
                {
                    Log("Got EOF");
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
