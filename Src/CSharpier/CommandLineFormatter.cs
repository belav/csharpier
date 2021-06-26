using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
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

            async Task<CommandLineFormatter?> CreateFormatter(string path)
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

                var printerOptions = ConfigurationFileOptions.CreatePrinterOptions(
                    baseDirectoryPath,
                    fileSystem
                );

                var ignoreFile = await IgnoreFile.Create(
                    baseDirectoryPath,
                    fileSystem,
                    console,
                    cancellationToken
                );
                if (ignoreFile is null)
                {
                    return null;
                }

                return new CommandLineFormatter(
                    baseDirectoryPath,
                    normalizedPath,
                    commandLineOptions,
                    printerOptions,
                    fileSystem,
                    console,
                    ignoreFile,
                    result
                );
            }

            if (commandLineOptions.StandardInFileContents != null)
            {
                var path = commandLineOptions.DirectoryOrFilePaths[0];

                var commandLineFormatter = await CreateFormatter(path);
                if (commandLineFormatter == null)
                {
                    return 1;
                }

                await commandLineFormatter.FormatFile(
                    commandLineOptions.StandardInFileContents,
                    path,
                    console.InputEncoding,
                    cancellationToken
                );
            }
            else
            {
                foreach (var path in commandLineOptions.DirectoryOrFilePaths)
                {
                    var commandLineFormatter = await CreateFormatter(path);
                    if (commandLineFormatter == null)
                    {
                        return 1;
                    }

                    await commandLineFormatter.FormatFiles(cancellationToken);
                }
            }

            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            if (!commandLineOptions.ShouldWriteStandardOut)
            {
                ResultPrinter.PrintResults(result, console, commandLineOptions);
            }
            return ReturnExitCode(commandLineOptions, result);
        }

        public async Task FormatFiles(CancellationToken cancellationToken)
        {
            if (this.FileSystem.File.Exists(this.Path))
            {
                await FormatFileFromPath(this.Path, cancellationToken);
            }
            else
            {
                var tasks = this.FileSystem.Directory.EnumerateFiles(
                        this.Path,
                        "*.cs",
                        SearchOption.AllDirectories
                    )
                    .Select(o => FormatFileFromPath(o, cancellationToken))
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

        private async Task FormatFileFromPath(string filePath, CancellationToken cancellationToken)
        {
            if (ShouldIgnoreFile(filePath))
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var (encoding, fileContents, unableToDetectEncoding) = await FileReader.ReadFile(
                filePath,
                this.FileSystem,
                cancellationToken
            );
            if (fileContents.Length == 0)
            {
                return;
            }

            if (unableToDetectEncoding)
            {
                WriteLine(
                    $"{GetPath(filePath)} - unable to detect file encoding. Defaulting to {encoding}"
                );
            }

            await FormatFile(fileContents, filePath, encoding, cancellationToken);
        }

        private async Task FormatFile(
            string fileContents,
            string filePath,
            Encoding encoding,
            CancellationToken cancellationToken
        ) {
            cancellationToken.ThrowIfCancellationRequested();

            CSharpierResult result;

            try
            {
                result = await new CodeFormatter().FormatAsync(
                    fileContents,
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
                WriteLine(GetPath(filePath) + " - threw exception while formatting");
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
                WriteLine();
                Interlocked.Increment(ref this.Result.ExceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref this.Result.Files);
                WriteLine(GetPath(filePath) + " - failed to compile");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref this.Result.Files);
                WriteLine(GetPath(filePath) + " - " + result.FailureMessage);
                return;
            }

            await PerformSyntaxTreeValidation(filePath, fileContents, result, cancellationToken);

            PerformCheck(filePath, result, fileContents);

            cancellationToken.ThrowIfCancellationRequested();
            Interlocked.Increment(ref this.Result.Files);

            WriteResult(filePath, result, fileContents, encoding);
        }

        private async Task PerformSyntaxTreeValidation(
            string file,
            string fileContents,
            CSharpierResult result,
            CancellationToken cancellationToken
        ) {
            if (!this.CommandLineOptions.Fast)
            {
                var syntaxNodeComparer = new SyntaxNodeComparer(
                    fileContents,
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
        }

        private void PerformCheck(string filePath, CSharpierResult result, string fileContents)
        {
            if (
                this.CommandLineOptions.Check
                && !this.CommandLineOptions.ShouldWriteStandardOut
                && result.Code != fileContents
            ) {
                WriteLine(GetPath(filePath) + " - was not formatted");
                StringDiffer.PrintFirstDifference(result.Code, fileContents, this.Console);
                Interlocked.Increment(ref this.Result.UnformattedFiles);
            }
        }

        private void WriteResult(
            string filePath,
            CSharpierResult result,
            string? fileContents,
            Encoding? encoding
        ) {
            if (this.CommandLineOptions.ShouldWriteStandardOut)
            {
                this.Console.Write(result.Code);
            }
            else
            {
                if (
                    !this.CommandLineOptions.Check
                    && !this.CommandLineOptions.SkipWrite
                    && result.Code != fileContents
                ) {
                    // purposely avoid async here, that way the file completely writes if the process gets cancelled while running.
                    this.FileSystem.File.WriteAllText(filePath, result.Code, encoding);
                }
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
            if (!this.CommandLineOptions.ShouldWriteStandardOut)
            {
                this.Console.WriteLine(line);
            }
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
