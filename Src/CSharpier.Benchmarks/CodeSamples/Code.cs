using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public class CodeFormatter
    {
        public CSharpierResult Format(string code, PrinterOptions printerOptions)
        {
            return this.FormatAsync(code, printerOptions, CancellationToken.None).Result;
        }

        public async Task<CSharpierResult> FormatAsync(
            string code,
            PrinterOptions printerOptions,
            CancellationToken cancellationToken
        ) {
            var syntaxTree = CSharpSyntaxTree.ParseText(
                code,
                new CSharpParseOptions(LanguageVersion.CSharp9, DocumentationMode.Diagnose),
                cancellationToken: cancellationToken
            );
            var syntaxNode = await syntaxTree.GetRootAsync(cancellationToken);
            if (syntaxNode is not CompilationUnitSyntax rootNode)
            {
                throw new Exception(
                    ""Root was not CompilationUnitSyntax, it was "" + syntaxNode.GetType()
                );
            }

            if (GeneratedCodeUtilities.BeginsWithAutoGeneratedComment(rootNode))
            {
                return new CSharpierResult { Code = code };
            }

            var diagnostics = syntaxTree.GetDiagnostics(cancellationToken)
                .Where(o => o.Severity == DiagnosticSeverity.Error && o.Id != ""CS1029"")
                .ToList();
            if (diagnostics.Any())
            {
                return new CSharpierResult
                {
                    Code = code,
                    Errors = diagnostics,
                    AST = printerOptions.IncludeAST ? this.PrintAST(rootNode) : string.Empty
                };
            }

            try
            {
                var document = Node.Print(rootNode);
                var lineEnding = GetLineEnding(code, printerOptions);
                var formattedCode = DocPrinter.DocPrinter.Print(
                    document,
                    printerOptions,
                    lineEnding,
                    code.Length
                );
                return new CSharpierResult
                {
                    Code = formattedCode,
                    DocTree = printerOptions.IncludeDocTree
                        ? DocSerializer.Serialize(document)
                        : string.Empty,
                    AST = printerOptions.IncludeAST ? this.PrintAST(rootNode) : string.Empty
                };
            }
            catch (InTooDeepException)
            {
                return new CSharpierResult
                {
                    FailureMessage = ""We can't handle this deep of recursion yet.""
                };
            }
        }

        public static string GetLineEnding(string code, PrinterOptions printerOptions)
        {
            if (printerOptions.EndOfLine == EndOfLine.Auto)
            {
                var lineIndex = code.IndexOf('\n');
                if (lineIndex <= 0)
                {
                    return ""\n"";
                }
                if (code[lineIndex - 1] == '\r')
                {
                    return ""\r\n"";
                }

                return ""\n"";
            }

            return printerOptions.EndOfLine == EndOfLine.CRLF ? ""\r\n"" : ""\n"";
        }

        private string PrintAST(CompilationUnitSyntax rootNode)
        {
            var stringBuilder = new StringBuilder();
            SyntaxNodeJsonWriter.WriteCompilationUnitSyntax(stringBuilder, rootNode);
            return JsonConvert.SerializeObject(
                JsonConvert.DeserializeObject(stringBuilder.ToString()),
                Formatting.Indented
            );
        }
    }

    public class CSharpierResult
    {
        public string Code { get; set; } = string.Empty;
        public string DocTree { get; set; } = string.Empty;
        public string AST { get; set; } = string.Empty;
        public IEnumerable<Diagnostic> Errors { get; set; } = Enumerable.Empty<Diagnostic>();

        public string FailureMessage { get; set; } = string.Empty;
    }

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
        }

        public async Task<int> Format(CancellationToken cancellationToken)
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
                        this.RootPath,
                        ""*.cs"",
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
            var ignoreFilePath = Path.Combine(this.RootPath, "".csharpierignore"");
            if (this.FileSystem.File.Exists(ignoreFilePath))
            {
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
                            ""The .csharpierignore file at ""
                            + ignoreFilePath
                            + "" could not be parsed due to the following line:""
                        );
                        WriteLine(line);
                        WriteLine(""Exception: "" + ex.Message);
                        return 1;
                    }
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
                    $""{GetPath(file)} - unable to detect file encoding. Defaulting to {fileReaderResult.Encoding}""
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
                WriteLine(GetPath(file) + "" - threw exception while formatting"");
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
                WriteLine();
                Interlocked.Increment(ref this.ExceptionsFormatting);
                return;
            }

            if (result.Errors.Any())
            {
                Interlocked.Increment(ref this.Files);
                WriteLine(GetPath(file) + "" - failed to compile"");
                return;
            }

            if (!result.FailureMessage.IsBlank())
            {
                Interlocked.Increment(ref this.Files);
                WriteLine(GetPath(file) + "" - "" + result.FailureMessage);
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
                        WriteLine(GetPath(file) + "" - failed syntax tree validation"");
                        WriteLine(failure);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref this.ExceptionsValidatingSource);
                    WriteLine(
                        GetPath(file)
                        + "" - failed with exception during syntax tree validation""
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
                    WriteLine(GetPath(file) + "" - was not formatted"");
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
            return PadToSize(file.Substring(this.RootPath.Length));
        }

        private void PrintResults()
        {
            WriteLine(
                PadToSize(""total time: "", 80) + ReversePad(Stopwatch.ElapsedMilliseconds + ""ms"")
            );
            PrintResultLine(""Total files"", Files);

            if (!this.CommandLineOptions.Fast)
            {
                PrintResultLine(""Failed syntax tree validation"", FailedSyntaxTreeValidation);

                PrintResultLine(""Threw exceptions while formatting"", ExceptionsFormatting);
                PrintResultLine(
                    ""files that threw exceptions while validating syntax tree"",
                    ExceptionsValidatingSource
                );
            }

            if (this.CommandLineOptions.Check)
            {
                PrintResultLine(""files that were not formatted"", UnformattedFiles);
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
            this.WriteLine(PadToSize(message + "": "", 80) + ReversePad(count + ""  ""));
        }

        // this could be implemented with a https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-5.0
        // when we implement include/exclude/ignore for real, look into using that
        private bool IgnoreFile(string filePath)
        {
            if (GeneratedCodeUtilities.IsGeneratedCodeFile(filePath))
            {
                return true;
            }

            var normalizedFilePath = filePath.Replace(""\\"", ""/"")
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
                value += "" "";
            }

            return value;
        }

        private static string ReversePad(string value)
        {
            while (value.Length < 10)
            {
                value = "" "" + value;
            }

            return value;
        }
    }
}
"