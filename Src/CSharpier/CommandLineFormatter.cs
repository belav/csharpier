using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    public class CommandLineFormatter
    {
        protected int FailedSyntaxTreeValidation;
        protected int ExceptionsFormatting;
        protected int ExceptionsValidatingSource;
        protected int Files;
        protected int UnformattedFiles;

        protected readonly string RootPath;
        protected readonly bool Validate;
        protected readonly bool Check;
        protected readonly bool SkipWrite;
        protected readonly Stopwatch Stopwatch;

        protected readonly ConfigurationOptions ConfigurationOptions;

        private CommandLineFormatter(
            string rootPath,
            bool check,
            bool fast,
            bool skipWrite
        ) {
            this.RootPath = rootPath;
            this.ConfigurationOptions = ConfigurationOptions.Create(rootPath);
            this.Check = check;
            this.Validate = !fast;
            this.SkipWrite = skipWrite;
            this.Stopwatch = Stopwatch.StartNew();
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

            var commandLineFormatter = new CommandLineFormatter(
                rootPath,
                check,
                fast,
                skipWrite
            );
            return await commandLineFormatter.Format(
                directoryOrFile,
                cancellationToken
            );
        }

        private async Task<int> Format(
            string directoryOrFile,
            CancellationToken cancellationToken
        ) {
            if (File.Exists(directoryOrFile))
            {
                await FormatFile(directoryOrFile, cancellationToken);
            }
            else
            {
                var tasks = Directory.EnumerateFiles(
                        this.RootPath,
                        "*.cs",
                        SearchOption.AllDirectories
                    )
                    .Select(o => FormatFile(o, cancellationToken))
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

            PrintResults();

            return ReturnExitCode();
        }

        private async Task FormatFile(
            string file,
            CancellationToken cancellationToken
        ) {
            if (IgnoreFile(file))
            {
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
                    $"{GetPath(file)} - unable to detect file encoding. Defaulting to {fileReaderResult.Encoding}"
                );
            }

            cancellationToken.ThrowIfCancellationRequested();

            CSharpierResult result;

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
                Interlocked.Increment(ref this.Files);
                Console.WriteLine(
                    GetPath(file) + " - threw exception while formatting"
                );
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                Interlocked.Increment(ref this.ExceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref this.Files);
                Console.WriteLine(GetPath(file) + " - failed to compile");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref this.Files);
                Console.WriteLine(GetPath(file) + " - " + result.FailureMessage);
                return;
            }

            if (this.Validate)
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
                        Interlocked.Increment(
                            ref this.FailedSyntaxTreeValidation
                        );
                        Console.WriteLine(
                            GetPath(file) + " - failed syntax tree validation"
                        );
                        Console.WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref this.ExceptionsValidatingSource);
                    Console.WriteLine(
                        GetPath(file)
                        + " - failed with exception during syntax tree validation"
                        + Environment.NewLine
                        + ex.Message
                        + ex.StackTrace
                    );
                }
            }

            if (this.Check)
            {
                if (result.Code != fileReaderResult.FileContents)
                {
                    Console.WriteLine(GetPath(file) + " - was not formatted");
                    Interlocked.Increment(ref this.UnformattedFiles);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            Interlocked.Increment(ref this.Files);

            if (!this.Check)
            {
                // purposely avoid async here, that way the file completely writes if the process gets cancelled while running.
                File.WriteAllText(file, result.Code, fileReaderResult.Encoding);
            }
        }

        private string GetPath(string file)
        {
            return PadToSize(file.Substring(this.RootPath.Length));
        }
        
        private void PrintResults()
        {
            Console.WriteLine(
                PadToSize("total time: ", 80)
                + ReversePad(Stopwatch.ElapsedMilliseconds + "ms")
            );
            PrintResultLine("Total files", Files);

            if (Validate)
            {
                PrintResultLine(
                    "Failed syntax tree validation",
                    FailedSyntaxTreeValidation
                );

                PrintResultLine(
                    "Threw exceptions while formatting",
                    ExceptionsFormatting
                );
                PrintResultLine(
                    "files that threw exceptions while validating syntax tree",
                    ExceptionsValidatingSource
                );
            }

            if (Check)
            {
                PrintResultLine(
                    "files that were not formatted",
                    UnformattedFiles
                );
            }
        }

        private int ReturnExitCode()
        {
            if (
                (Check && UnformattedFiles > 0)
                || FailedSyntaxTreeValidation > 0
                || ExceptionsFormatting > 0
                || ExceptionsValidatingSource > 0
            ) {
                return 1;
            }

            return 0;
        }

        private static void PrintResultLine(string message, int count)
        {
            Console.WriteLine(
                PadToSize(message + ": ", 80) + ReversePad(count + "  ")
            );
        }

        // TODO this could be implemented with a https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-5.0
        // when we implement include/exclude/ignore for real, look into using that
        private bool IgnoreFile(string filePath)
        {
            var normalizedFilePath = filePath.Replace("\\", "/")
                .Substring(this.RootPath.Length + 1);

            if (
                this.ConfigurationOptions.Exclude.Contains(normalizedFilePath)
            ) {
                return true;
            }

            return normalizedFilePath.EndsWith(".g.cs")
            || normalizedFilePath.EndsWith(".cshtml.cs")
            || normalizedFilePath.ContainsIgnoreCase("/obj/");
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
