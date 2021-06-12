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
                var input = new StringBuilder();
                var value = 0;
                while ((value = Console.Read()) != -1)
                {
                    input.Append(Convert.ToChar(value));
                }

                standardInFileContents = input.ToString();
            }

            if (directoryOrFileNotProvided)
            {
                directoryOrFile = new[] { Directory.GetCurrentDirectory() };
            }
            else
            {
                if (standardInFileContents != null)
                {
                    Console.WriteLine(
                        "directoryOrFile may not be supplied when piping standard input"
                    );
                    return 1;
                }

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
