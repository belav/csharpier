using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ignore = Ignore.Ignore;

namespace CSharpier
{
    public class CommandLineFormatter
    {
        protected int FailedSyntaxTreeValidation;
        protected int ExceptionsFormatting;
        protected int ExceptionsValidatingSource;
        protected int Files;
        protected int UnformattedFiles;

        protected readonly Stopwatch Stopwatch;

        protected readonly string RootPath;

        protected readonly CommandLineOptions CommandLineOptions;
        protected readonly PrinterOptions PrinterOptions;
        protected readonly IFileSystem FileSystem;

        public global::Ignore.Ignore Ignore { get; set; }

        public CommandLineFormatter(
            string rootPath,
            CommandLineOptions commandLineOptions,
            PrinterOptions printerOptions,
            IFileSystem fileSystem
        ) {
            this.RootPath = rootPath;
            this.PrinterOptions = printerOptions;
            this.CommandLineOptions = commandLineOptions;
            this.Stopwatch = Stopwatch.StartNew();
            this.FileSystem = fileSystem;
            this.Ignore = new global::Ignore.Ignore();

            var ignoreFilePath = Path.Combine(
                this.RootPath,
                ".csharpierignore"
            );
            if (this.FileSystem.File.Exists(ignoreFilePath))
            {
                this.Ignore.Add(
                    this.FileSystem.File.ReadAllLines(ignoreFilePath)
                );
            }
        }

        public async Task<int> Format(CancellationToken cancellationToken)
        {
            if (
                this.FileSystem.File.Exists(
                    this.CommandLineOptions.DirectoryOrFile
                )
            ) {
                await FormatFile(
                    this.CommandLineOptions.DirectoryOrFile,
                    cancellationToken
                );
            }
            else
            {
                var tasks = this.FileSystem.Directory.EnumerateFiles(
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
                await FileReader.ReadFile(
                    file,
                    this.FileSystem,
                    cancellationToken
                );
            if (fileReaderResult.FileContents.Length == 0)
            {
                return;
            }
            if (fileReaderResult.DefaultedEncoding)
            {
                WriteLine(
                    $"{GetPath(file)} - unable to detect file encoding. Defaulting to {fileReaderResult.Encoding}"
                );
            }

            cancellationToken.ThrowIfCancellationRequested();

            CSharpierResult result;

            try
            {
                result = await new CodeFormatter().FormatAsync(
                    fileReaderResult.FileContents,
                    this.PrinterOptions,
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
                WriteLine(
                    GetPath(file) + " - threw exception while formatting"
                );
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
                WriteLine();
                Interlocked.Increment(ref this.ExceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref this.Files);
                WriteLine(GetPath(file) + " - failed to compile");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref this.Files);
                WriteLine(GetPath(file) + " - " + result.FailureMessage);
                return;
            }

            if (!this.CommandLineOptions.Fast)
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
                        WriteLine(
                            GetPath(file) + " - failed syntax tree validation"
                        );
                        WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref this.ExceptionsValidatingSource);
                    WriteLine(
                        GetPath(file) +
                        " - failed with exception during syntax tree validation" +
                        Environment.NewLine +
                        ex.Message +
                        ex.StackTrace
                    );
                }
            }

            if (this.CommandLineOptions.Check)
            {
                if (result.Code != fileReaderResult.FileContents)
                {
                    WriteLine(GetPath(file) + " - was not formatted");
                    Interlocked.Increment(ref this.UnformattedFiles);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            Interlocked.Increment(ref this.Files);

            if (
                !this.CommandLineOptions.Check &&
                !this.CommandLineOptions.SkipWrite
            ) {
                // purposely avoid async here, that way the file completely writes if the process gets cancelled while running.
                this.FileSystem.File.WriteAllText(
                    file,
                    result.Code,
                    fileReaderResult.Encoding
                );
            }
        }

        private string GetPath(string file)
        {
            return PadToSize(file.Substring(this.RootPath.Length));
        }

        private void PrintResults()
        {
            WriteLine(
                PadToSize("total time: ", 80) +
                ReversePad(Stopwatch.ElapsedMilliseconds + "ms")
            );
            PrintResultLine("Total files", Files);

            if (!this.CommandLineOptions.Fast)
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

            if (this.CommandLineOptions.Check)
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
                (this.CommandLineOptions.Check && UnformattedFiles > 0) ||
                FailedSyntaxTreeValidation > 0 ||
                ExceptionsFormatting > 0 ||
                ExceptionsValidatingSource > 0
            ) {
                return 1;
            }

            return 0;
        }

        private void PrintResultLine(string message, int count)
        {
            this.WriteLine(
                PadToSize(message + ": ", 80) + ReversePad(count + "  ")
            );
        }

        // TODO this could be implemented with a https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-5.0
        // when we implement include/exclude/ignore for real, look into using that
        private bool IgnoreFile(string filePath)
        {
            if (GeneratedCodeUtilities.IsGeneratedCodeFile(filePath))
            {
                return true;
            }

            var normalizedFilePath = filePath.Replace("\\", "/")
                .Substring(this.RootPath.Length + 1);

            return this.Ignore.IsIgnored(normalizedFilePath);
        }

        protected virtual void WriteLine(string? line = null)
        {
            Console.WriteLine(line);
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
