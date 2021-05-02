using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    class Program
    {
        // TODO 1 Configuration.cs from data.entities
        // TODO 1 CurrencyDto.cs from data.entities

        // TODO https://github.com/dotnet/command-line-api/blob/main/docs/Your-first-app-with-System-CommandLine-DragonFruit.md
        // can be used to simplify this, but it didn't appear to work with descriptions of the parameters
        static async Task<int> Main(string[] args)
        {
            var rootCommand = CommandLineOptions.Create();

            rootCommand.Handler = CommandHandler.Create(
                new CommandLineOptions.Handler(Run)
            );

            return await rootCommand.InvokeAsync(args);
        }

        public static async Task<int> Run(
            string directoryOrFile,
            bool check,
            bool fast,
            bool skipWrite,
            CancellationToken cancellationToken
        ) {
            if (string.IsNullOrEmpty(directoryOrFile))
            {
                directoryOrFile = Directory.GetCurrentDirectory();
            }

            var rootPath = File.Exists(directoryOrFile)
                ? Path.GetDirectoryName(directoryOrFile)
                : directoryOrFile;

            if (rootPath == null)
            {
                throw new Exception(
                    "The path of " +
                    directoryOrFile +
                    " does not appear to point to a directory or a file."
                );
            }

            var configurationFileOptions = ConfigurationFileOptions.Create(
                rootPath,
                new FileSystem()
            );

            var printerOptions = new PrinterOptions
            {
                TabWidth = configurationFileOptions.TabWidth,
                UseTabs = configurationFileOptions.UseTabs,
                Width = configurationFileOptions.PrintWidth,
                EndOfLine = configurationFileOptions.EndOfLine == "lf"
                    ? "\n"
                    : configurationFileOptions.EndOfLine == "crlf"
                            ? "\r\n"
                            : throw new Exception(
                                    "Unhandled value from EndOfLine options " +
                                    configurationFileOptions.EndOfLine
                                )
            };

            var commandLineFormatter = new CommandLineFormatter(
                rootPath,
                check,
                fast,
                skipWrite,
                configurationFileOptions.Exclude,
                printerOptions
            );
            return await commandLineFormatter.Format(
                directoryOrFile,
                cancellationToken
            );
        }
    }
}
