using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CSharpier
{
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
            CancellationToken cancellationToken
        ) {
            var console = new SystemConsole();
            var logger = new ConsoleLogger(console);

            var directoryOrFileNotProvided = (directoryOrFile is null or { Length: 0 });

            string? standardInFileContents = null;
            if (Console.IsInputRedirected && directoryOrFileNotProvided)
            {
                using var streamReader = new StreamReader(
                    Console.OpenStandardInput(),
                    Console.InputEncoding
                );
                standardInFileContents = await streamReader.ReadToEndAsync();

                directoryOrFile = new[] { Directory.GetCurrentDirectory() };
            }
            else
            {
                directoryOrFile = directoryOrFile!.Select(
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
                WriteStdout = writeStdout
            };

            return await CommandLineFormatter.Format(
                commandLineOptions,
                new FileSystem(),
                console,
                logger,
                cancellationToken
            );
        }
    }
}
