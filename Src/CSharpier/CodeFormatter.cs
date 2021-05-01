using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace CSharpier
{
    public class CodeFormatter
    {
        public CSharpierResult Format(string code, Options options)
        {
            return this.FormatAsync(
                code,
                options,
                CancellationToken.None
            ).Result;
        }

        public async Task<CSharpierResult> FormatAsync(
            string code,
            Options options,
            CancellationToken cancellationToken
        ) {
            var syntaxTree = CSharpSyntaxTree.ParseText(
                code,
                new CSharpParseOptions(
                    LanguageVersion.CSharp9,
                    DocumentationMode.Diagnose
                ),
                cancellationToken: cancellationToken
            );
            var syntaxNode = await syntaxTree.GetRootAsync(cancellationToken);
            if (syntaxNode is not CompilationUnitSyntax rootNode)
            {
                throw new Exception(
                    "Root was not CompilationUnitSyntax, it was " +
                    syntaxNode.GetType()
                );
            }

            var diagnostics = syntaxTree.GetDiagnostics(cancellationToken)
                .Where(
                    o =>
                        o.Severity == DiagnosticSeverity.Error &&
                        o.Id != "CS1029"
                )
                .ToList();
            if (diagnostics.Any())
            {
                return new CSharpierResult
                {
                    Code = code,
                    Errors = diagnostics,
                    AST = options.IncludeAST
                        ? this.PrintAST(rootNode)
                        : string.Empty
                };
            }

            try
            {
                var document = Node.Print(rootNode);
                var formattedCode = DocPrinter.Print(document, options);
                return new CSharpierResult
                {
                    Code = formattedCode,
                    DocTree = options.IncludeDocTree
                        ? DocTreePrinter.Print(document)
                        : string.Empty,
                    AST = options.IncludeAST
                        ? this.PrintAST(rootNode)
                        : string.Empty
                };
            }
            catch (InTooDeepException)
            {
                return new CSharpierResult
                {
                    FailureMessage = "We can't handle this deep of recursion yet."
                };
            }
        }

        private string PrintAST(CompilationUnitSyntax rootNode)
        {
            var stringBuilder = new StringBuilder();
            SyntaxNodeJsonWriter.WriteCompilationUnitSyntax(
                stringBuilder,
                rootNode
            );
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
        public IEnumerable<Diagnostic> Errors { get; set; } =
            Enumerable.Empty<Diagnostic>();

        public string FailureMessage { get; set; } = string.Empty;
    }
}
