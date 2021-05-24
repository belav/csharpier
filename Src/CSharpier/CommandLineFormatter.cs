using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpier.Utilities;

namespace CSharpier
{
    public class CommandLineFormatter
    {
        protected readonly CommandLineFormatterResult Result;

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
            IgnoreFile ignoreFile,
            CommandLineFormatterResult result
        ) {
            this.BaseDirectoryPath = baseDirectoryPath;
            this.Path = path;
            this.PrinterOptions = printerOptions;
            this.CommandLineOptions = commandLineOptions;
            this.FileSystem = fileSystem;
            this.Console = console;
            this.IgnoreFile = ignoreFile;
            this.Result = result;
        }

        public static async Task<int> Format(
            CommandLineOptions commandLineOptions,
            IFileSystem fileSystem,
            IConsole console,
            CancellationToken cancellationToken
        ) {
            var stopwatch = Stopwatch.StartNew();
            var result = new CommandLineFormatterResult();

            foreach (var path in commandLineOptions.DirectoryOrFilePaths)
            {
                var normalizedPath = path.Replace('\\', '/');
                var baseDirectoryPath = fileSystem.File.Exists(normalizedPath)
                    ? fileSystem.Path.GetDirectoryName(normalizedPath)
                    : path;

                if (baseDirectoryPath == null)
                {
                    throw new Exception(
                        $"The path of {normalizedPath} does not appear to point to a directory or a file."
                    );
                }

                var configurationFileOptions = ConfigurationFileOptions.Create(
                    baseDirectoryPath,
                    fileSystem
                );

                var ignoreFile =
                    await IgnoreFile.Create(
                        baseDirectoryPath,
                        fileSystem,
                        console,
                        cancellationToken
                    );
                if (ignoreFile is null)
                {
                    return 1;
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
                    normalizedPath,
                    commandLineOptions,
                    printerOptions,
                    fileSystem,
                    console,
                    ignoreFile,
                    result
                );

                await commandLineFormatter.FormatFiles(cancellationToken);
            }

            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            ResultPrinter.PrintResults(result, console, commandLineOptions);
            return ReturnExitCode(commandLineOptions, result);
        }

        public async Task FormatFiles(CancellationToken cancellationToken)
        {
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
                Interlocked.Increment(ref this.Result.Files);
                WriteLine(GetPath(file) + " - threw exception while formatting");
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
                WriteLine();
                Interlocked.Increment(ref this.Result.ExceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref this.Result.Files);
                WriteLine(GetPath(file) + " - failed to compile");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref this.Result.Files);
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
                        Interlocked.Increment(ref this.Result.FailedSyntaxTreeValidation);
                        WriteLine(GetPath(file) + " - failed syntax tree validation");
                        WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref this.Result.ExceptionsValidatingSource);
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
                    StringDiffer.PrintDifference(
                        result.Code,
                        fileReaderResult.FileContents,
                        this.Console
                    );
                    Interlocked.Increment(ref this.Result.UnformattedFiles);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            Interlocked.Increment(ref this.Result.Files);

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
        // these are public fields so that Interlocked.Increment may be used on them.
        public int FailedSyntaxTreeValidation;
        public int ExceptionsFormatting;
        public int ExceptionsValidatingSource;
        public int Files;
        public int UnformattedFiles;
        public long ElapsedMilliseconds;
    }
}
