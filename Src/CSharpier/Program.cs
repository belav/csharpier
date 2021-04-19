using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtfUnknown;

namespace CSharpier
{
    class Program
    {
        private static int sourceLost;
        private static int exceptionsFormatting;
        private static int exceptionsValidatingSource;
        private static int files;
        private static int unformattedFiles;

        static async Task<int> Main(string[] args)
        {
            var rootCommand = CommandLineOptions.Create();

            rootCommand.Handler = CommandHandler.Create(
                new CommandLineOptions.Handler(Run)
            );

            return await rootCommand.InvokeAsync(args);
        }

        // TODO look into https://github.com/dotnet/command-line-api/blob/main/docs/Your-first-app-with-System-CommandLine-DragonFruit.md the next time options are added
        public static async Task<int> Run(
            string directoryOrFile,
            bool fast,
            bool check,
            CancellationToken cancellationToken
        ) {
            var fullStopwatch = Stopwatch.StartNew();

            // TODO 1 Configuration.cs from data.entities
            // TODO 1 CurrencyDto.cs from data.entities
            if (string.IsNullOrEmpty(directoryOrFile))
            {
                directoryOrFile = Directory.GetCurrentDirectory();
            }

            var validate = !fast;

            if (File.Exists(directoryOrFile))
            {
                await DoWork(
                    directoryOrFile,
                    Path.GetDirectoryName(directoryOrFile),
                    validate,
                    check,
                    cancellationToken
                );
            }
            else
            {
                var tasks = Directory.EnumerateFiles(
                        directoryOrFile,
                        "*.cs",
                        SearchOption.AllDirectories
                    )
                    .Select(
                        o =>
                            DoWork(
                                o,
                                directoryOrFile,
                                validate,
                                check,
                                cancellationToken
                            )
                    )
                    .ToArray();
                try
                {
                    Task.WaitAll(tasks, cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken != cancellationToken)
                    {
                        throw;
                    }
                }
            }

            Console.WriteLine(
                PadToSize("total time: ", 80)
                + ReversePad(fullStopwatch.ElapsedMilliseconds + "ms")
            );
            Console.WriteLine(
                PadToSize("total files: ", 80) + ReversePad(files + "  ")
            );
            if (validate)
            {
                Console.WriteLine(
                    PadToSize("files that failed syntax tree validation: ", 80)
                    + ReversePad(sourceLost + "  ")
                );
                Console.WriteLine(
                    PadToSize(
                        "files that threw exceptions while formatting: ",
                        80
                    )
                    + ReversePad(exceptionsFormatting + "  ")
                );
                Console.WriteLine(
                    PadToSize(
                        "files that threw exceptions while validating syntax tree: ",
                        80
                    )
                    + ReversePad(exceptionsValidatingSource + "  ")
                );
            }

            if (check)
            {
                Console.WriteLine(
                    PadToSize("files that were not formatted: ", 80)
                    + ReversePad(unformattedFiles + "  ")
                );

                if (unformattedFiles > 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static async Task DoWork(
            string file,
            string? path,
            bool validate,
            bool check,
            CancellationToken cancellationToken
        ) {
            if (
                file.EndsWith(".g.cs")
                || file.EndsWith(".cshtml.cs")
                || file.ContainsIgnoreCase("\\obj\\")
                || file.ContainsIgnoreCase("/obj/")
                || file.EndsWithIgnoreCase("AllInOne.cs")
            ) {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var fileReaderResult =
                await FileReader.ReadFile(file, cancellationToken);
            if (fileReaderResult.FileContents.Length == 0)
            {
                return;
            }
            if (fileReaderResult.DefaultedEncoding)
            {
                Console.WriteLine(
                    $"{GetPath()} - unable to detect file encoding. Defaulting to {fileReaderResult.Encoding}"
                );
            }

            cancellationToken.ThrowIfCancellationRequested();

            CSharpierResult result;

            string GetPath()
            {
                return PadToSize(file.Substring(path?.Length ?? 0));
            }

            try
            {
                result = await new CodeFormatter().FormatAsync(
                    fileReaderResult.FileContents,
                    new Options(),
                    cancellationToken
                );
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref files);
                Console.WriteLine(
                    GetPath() + " - threw exception while formatting"
                );
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                Interlocked.Increment(ref exceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref files);
                Console.WriteLine(GetPath() + " - failed to compile");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref files);
                Console.WriteLine(GetPath() + " - " + result.FailureMessage);
                return;
            }

            if (validate)
            {
                var syntaxNodeComparer = new SyntaxNodeComparer(
                    fileReaderResult.FileContents,
                    result.Code,
                    cancellationToken
                );

                try
                {
                    var failure =
                        await syntaxNodeComparer.CompareSourceAsync(
                            cancellationToken
                        );
                    if (!string.IsNullOrEmpty(failure))
                    {
                        Interlocked.Increment(ref sourceLost);
                        Console.WriteLine(
                            GetPath() + " - failed syntax tree validation"
                        );
                        Console.WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref exceptionsValidatingSource);
                    Console.WriteLine(
                        GetPath()
                        + " - failed with exception during syntax tree validation"
                        + Environment.NewLine
                        + ex.Message
                        + ex.StackTrace
                    );
                }
            }

            if (check)
            {
                if (result.Code != fileReaderResult.FileContents)
                {
                    Console.WriteLine(GetPath() + " - was not formatted");
                    Interlocked.Increment(ref unformattedFiles);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            Interlocked.Increment(ref files);

            if (!check)
            {
                // purposely avoid async here, that way the file completely writes if the process gets cancelled while running.
                File.WriteAllText(file, result.Code, fileReaderResult.Encoding);
            }
        }

        private static bool IgnoreFile(string filePath)
        {
            var normalizedFilePath = filePath.Replace("\\", "/");

            return normalizedFilePath.EndsWith(".g.cs")
            || normalizedFilePath.EndsWith(".cshtml.cs")
            || normalizedFilePath.ContainsIgnoreCase("/obj/")
            // can't format because of #121
            || normalizedFilePath.EndsWith(
                "efcore/test/Microsoft.Data.Sqlite.Tests/TestUtilities/SqliteTestFramework.cs"
            );
        }

        private static string PadToSize(string value, int size = 120)
        {
            while (value.Length < size)
            {
                value += " ";
            }

            return value;
        }

        private static string ReversePad(string value)
        {
            while (value.Length < 10)
            {
                value = " " + value;
            }

            return value;
        }
    }
}
