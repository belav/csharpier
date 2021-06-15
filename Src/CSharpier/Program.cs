using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;

namespace CSharpier
{
    class Program
    {
        // TODO https://github.com/dotnet/command-line-api/blob/main/docs/Your-first-app-with-System-CommandLine-DragonFruit.md
        // can be used to simplify this, but it didn't appear to work with descriptions of the parameters
        static async Task<int> Main(string[] args)
        {
            var rootCommand = CommandLineOptions.Create();

            rootCommand.Handler = CommandHandler.Create(new CommandLineOptions.Handler(Run));

            return await rootCommand.InvokeAsync(args);
        }

        public static async Task<int> Run(
            string[] directoryOrFile,
            bool check,
            bool fast,
            bool skipWrite,
            bool writeStdout,
            CancellationToken cancellationToken
        ) {
            var directoryOrFileNotProvided = (directoryOrFile is null or { Length: 0 });

            string? standardInFileContents = null;
            if (Console.IsInputRedirected && directoryOrFileNotProvided)
            {
                using var streamReader = new StreamReader(
                    Console.OpenStandardInput(),
                    Console.InputEncoding
                );
                standardInFileContents = await streamReader.ReadToEndAsync();
            }

            if (directoryOrFileNotProvided)
            {
                directoryOrFile = new[] { Directory.GetCurrentDirectory() };
            }
            else
            {
                directoryOrFile = directoryOrFile!.Select(
                        o => Path.Combine(Directory.GetCurrentDirectory(), o)
                    )
                    .ToArray();
            }

            var commandLineOptions = new CommandLineOptions
            {
                DirectoryOrFilePaths = directoryOrFile.ToArray(),
                StandardInFileContents = standardInFileContents,
                Check = check,
                Fast = fast,
                SkipWrite = skipWrite,
                WriteStdout = writeStdout
            };

            return await CommandLineFormatter.Format(
                commandLineOptions,
                new FileSystem(),
                new SystemConsole(),
                cancellationToken
            );
        }
    }
}
