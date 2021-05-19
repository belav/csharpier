using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
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

        protected readonly Stopwatch Stopwatch;

        protected readonly string BaseDirectoryPath;
        protected readonly string Path;
        protected readonly CommandLineOptions CommandLineOptions;
        protected readonly PrinterOptions PrinterOptions;
        protected readonly IFileSystem FileSystem;
        protected readonly IConsole Console;
        protected readonly IgnoreFile IgnoreFile;

        protected CommandLineFormatter(
            string baseDirectoryPath,
            string path,
            CommandLineOptions commandLineOptions,
            PrinterOptions printerOptions,
            IFileSystem fileSystem,
            IConsole console,
            IgnoreFile ignoreFile
        ) {
            this.BaseDirectoryPath = baseDirectoryPath;
            this.Path = path;
            this.PrinterOptions = printerOptions;
            this.CommandLineOptions = commandLineOptions;
            this.Stopwatch = Stopwatch.StartNew();
            this.FileSystem = fileSystem;
            this.Console = console;
            this.IgnoreFile = ignoreFile;
        }

        public static async Task<int> Format(
            CommandLineOptions commandLineOptions,
            IFileSystem fileSystem,
            IConsole console,
            CancellationToken cancellationToken
        ) {
            var result = new CommandLineFormatterResult();

            foreach (var path in commandLineOptions.Paths)
            {
                var baseDirectoryPath = fileSystem.File.Exists(path)
                    ? fileSystem.Path.GetDirectoryName(path)
                    : path;

                if (baseDirectoryPath == null)
                {
                    throw new Exception(
                        $"The path of {path} does not appear to point to a directory or a file."
                    );
                }

                // TODO write some tests, I am pretty sure options and ignore will be fine, but double check
                var configurationFileOptions = ConfigurationFileOptions.Create(
                    baseDirectoryPath,
                    fileSystem
                );

                var (ignoreFile, exitCode) = await IgnoreFile.Create(
                    baseDirectoryPath,
                    fileSystem,
                    console,
                    cancellationToken
                );
                if (exitCode != 0)
                {
                    return exitCode;
                }

                var printerOptions = new PrinterOptions
                {
                    TabWidth = configurationFileOptions.TabWidth,
                    UseTabs = configurationFileOptions.UseTabs,
                    Width = configurationFileOptions.PrintWidth,
                    EndOfLine = configurationFileOptions.EndOfLine
                };

                var commandLineFormatter = new CommandLineFormatter(
                    baseDirectoryPath,
                    path,
                    commandLineOptions,
                    printerOptions,
                    fileSystem,
                    console,
                    ignoreFile!
                );

                var nextResult = await commandLineFormatter.FormatFiles(cancellationToken);
                // TODO we could maybe pass a result into the FormatFiles method, but would that work with Interlocked.Increment ?
                result.Files += nextResult.Files;
                result.ElapsedMilliseconds += nextResult.ElapsedMilliseconds;
                result.ExceptionsFormatting += nextResult.ExceptionsFormatting;
                result.UnformattedFiles += nextResult.UnformattedFiles;
                result.ExceptionsValidatingSource += nextResult.ExceptionsValidatingSource;
                result.FailedSyntaxTreeValidation += nextResult.FailedSyntaxTreeValidation;
            }

            ResultPrinter.PrintResults(result, console, commandLineOptions);
            return ReturnExitCode(commandLineOptions, result);
        }

        public async Task<CommandLineFormatterResult> FormatFiles(
            CancellationToken cancellationToken
        ) {
            if (this.FileSystem.File.Exists(this.Path))
            {
                await FormatFile(this.Path, cancellationToken);
            }
            else
            {
                var tasks = this.FileSystem.Directory.EnumerateFiles(
                        this.Path,
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

            return new CommandLineFormatterResult
            {
                FailedSyntaxTreeValidation = FailedSyntaxTreeValidation,
                ExceptionsFormatting = ExceptionsFormatting,
                ExceptionsValidatingSource = ExceptionsValidatingSource,
                Files = Files,
                UnformattedFiles = UnformattedFiles,
                ElapsedMilliseconds = Stopwatch.ElapsedMilliseconds
            };
        }

        private async Task FormatFile(string file, CancellationToken cancellationToken)
        {
            if (ShouldIgnoreFile(file))
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
            return ResultPrinter.PadToSize(file.Substring(this.BaseDirectoryPath.Length));
        }

        private static int ReturnExitCode(
            CommandLineOptions commandLineOptions,
            CommandLineFormatterResult result
        ) {
            if (
                (commandLineOptions.Check && result.UnformattedFiles > 0)
                || result.FailedSyntaxTreeValidation > 0
                || result.ExceptionsFormatting > 0
                || result.ExceptionsValidatingSource > 0
            ) {
                return 1;
            }

            return 0;
        }

        private bool ShouldIgnoreFile(string filePath)
        {
            return GeneratedCodeUtilities.IsGeneratedCodeFile(filePath)
                || this.IgnoreFile.IsIgnored(filePath);
        }

        protected void WriteLine(string? line = null)
        {
            this.Console.WriteLine(line);
        }
    }

    public class CommandLineFormatterResult
    {
        public int FailedSyntaxTreeValidation { get; set; }
        public int ExceptionsFormatting { get; set; }
        public int ExceptionsValidatingSource { get; set; }
        public int Files { get; set; }
        public int UnformattedFiles { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
