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

        protected readonly string BaseDirectoryPath;

        protected readonly CommandLineOptions CommandLineOptions;
        protected readonly PrinterOptions PrinterOptions;
        protected readonly IFileSystem FileSystem;

        public global::Ignore.Ignore Ignore { get; init; }

        protected CommandLineFormatter(
            string baseDirectoryPath,
            CommandLineOptions commandLineOptions,
            PrinterOptions printerOptions,
            IFileSystem fileSystem
        ) {
            this.BaseDirectoryPath = baseDirectoryPath;
            this.PrinterOptions = printerOptions;
            this.CommandLineOptions = commandLineOptions;
            this.Stopwatch = Stopwatch.StartNew();
            this.FileSystem = fileSystem;
            this.Ignore = new global::Ignore.Ignore();
        }

        public static async Task<int> Format(
            CommandLineOptions commandLineOptions,
            IFileSystem fileSystem,
            CancellationToken cancellationToken
        ) {
            var baseDirectoryPath = File.Exists(commandLineOptions.DirectoryOrFile)
                ? Path.GetDirectoryName(commandLineOptions.DirectoryOrFile)
                : commandLineOptions.DirectoryOrFile;

            if (baseDirectoryPath == null)
            {
                throw new Exception(
                    $"The path of {commandLineOptions.DirectoryOrFile} does not appear to point to a directory or a file."
                );
            }

            var configurationFileOptions = ConfigurationFileOptions.Create(
                baseDirectoryPath,
                fileSystem
            );

            var printerOptions = new PrinterOptions
            {
                TabWidth = configurationFileOptions.TabWidth,
                UseTabs = configurationFileOptions.UseTabs,
                Width = configurationFileOptions.PrintWidth,
                EndOfLine = configurationFileOptions.EndOfLine
            };

            var commandLineFormatter = new CommandLineFormatter(
                baseDirectoryPath,
                commandLineOptions,
                printerOptions,
                fileSystem
            );
            return await commandLineFormatter.FormatFiles(cancellationToken);
        }

        public async Task<int> FormatFiles(CancellationToken cancellationToken)
        {
            var ignoreExitCode = await this.ParseIgnoreFile(cancellationToken);
            if (ignoreExitCode != 0)
            {
                return ignoreExitCode;
            }

            if (this.FileSystem.File.Exists(this.CommandLineOptions.DirectoryOrFile))
            {
                await FormatFile(this.CommandLineOptions.DirectoryOrFile, cancellationToken);
            }
            else
            {
                var tasks = this.FileSystem.Directory.EnumerateFiles(
                        this.BaseDirectoryPath,
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

        private async Task<int> ParseIgnoreFile(CancellationToken cancellationToken)
        {
            var directoryInfo = this.FileSystem.DirectoryInfo.FromDirectoryName(
                this.BaseDirectoryPath
            );
            var ignoreFilePath = this.FileSystem.Path.Combine(
                directoryInfo.FullName,
                ".csharpierignore"
            );
            while (!this.FileSystem.File.Exists(ignoreFilePath))
            {
                directoryInfo = directoryInfo.Parent;
                if (directoryInfo == null)
                {
                    return 0;
                }
                ignoreFilePath = this.FileSystem.Path.Combine(
                    directoryInfo.FullName,
                    ".csharpierignore"
                );
            }

            foreach (
                var line in await this.FileSystem.File.ReadAllLinesAsync(
                    ignoreFilePath,
                    cancellationToken
                )
            ) {
                try
                {
                    this.Ignore.Add(line);
                }
                catch (Exception ex)
                {
                    WriteLine(
                        $"The .csharpierignore file at {ignoreFilePath} could not be parsed due to the following line:"
                    );
                    WriteLine(line);
                    WriteLine($"Exception: {ex.Message}");
                    return 1;
                }
            }

            return 0;
        }

        private async Task FormatFile(string file, CancellationToken cancellationToken)
        {
            if (IgnoreFile(file))
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var fileReaderResult =
                await FileReader.ReadFile(file, this.FileSystem, cancellationToken);
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
                WriteLine(GetPath(file) + " - threw exception while formatting");
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
                    var failure = await syntaxNodeComparer.CompareSourceAsync(cancellationToken);
                    if (!string.IsNullOrEmpty(failure))
                    {
                        Interlocked.Increment(ref this.FailedSyntaxTreeValidation);
                        WriteLine(GetPath(file) + " - failed syntax tree validation");
                        WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref this.ExceptionsValidatingSource);
                    WriteLine(
                        GetPath(file)
                        + " - failed with exception during syntax tree validation"
                        + Environment.NewLine
                        + ex.Message
                        + ex.StackTrace
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

            if (!this.CommandLineOptions.Check && !this.CommandLineOptions.SkipWrite)
            {
                // purposely avoid async here, that way the file completely writes if the process gets cancelled while running.
                this.FileSystem.File.WriteAllText(file, result.Code, fileReaderResult.Encoding);
            }
        }

        private string GetPath(string file)
        {
            return PadToSize(file.Substring(this.BaseDirectoryPath.Length));
        }

        private void PrintResults()
        {
            WriteLine(
                PadToSize("total time: ", 80) + ReversePad(Stopwatch.ElapsedMilliseconds + "ms")
            );
            PrintResultLine("Total files", Files);

            if (!this.CommandLineOptions.Fast)
            {
                PrintResultLine("Failed syntax tree validation", FailedSyntaxTreeValidation);

                PrintResultLine("Threw exceptions while formatting", ExceptionsFormatting);
                PrintResultLine(
                    "files that threw exceptions while validating syntax tree",
                    ExceptionsValidatingSource
                );
            }

            if (this.CommandLineOptions.Check)
            {
                PrintResultLine("files that were not formatted", UnformattedFiles);
            }
        }

        private int ReturnExitCode()
        {
            if (
                (this.CommandLineOptions.Check && UnformattedFiles > 0)
                || FailedSyntaxTreeValidation > 0
                || ExceptionsFormatting > 0
                || ExceptionsValidatingSource > 0
            ) {
                return 1;
            }

            return 0;
        }

        private void PrintResultLine(string message, int count)
        {
            this.WriteLine(PadToSize(message + ": ", 80) + ReversePad(count + "  "));
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
                .Substring(this.BaseDirectoryPath.Length + 1);

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
