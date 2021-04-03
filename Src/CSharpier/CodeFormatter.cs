using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            CancellationToken cancellationToken)
        {
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
                    "Root was not CompilationUnitSyntax, it was " + syntaxNode.GetType()
                );
            }

            var diagnostics = syntaxTree.GetDiagnostics(cancellationToken)
                .Where(
                    o => o.Severity == DiagnosticSeverity.Error
                    && o.Id != "CS1029"
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
                var document = new Printer().Print(rootNode);
                var formattedCode = DocPrinter.Print(document, options);
                return new CSharpierResult
                {
                    Code = formattedCode,
                    DocTree = options.IncludeDocTree
                        ? this.PrintDocTree(document, string.Empty)
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

        private string PrintDocTree(Doc document, string indent)
        {
            switch (document)
            {
                case NullDoc:
                    return indent + "Doc.Null";
                case StringDoc stringDoc:
                    return indent + "\"" + stringDoc.Value?.Replace(
                        "\"",
                        "\\\""
                    ) + "\"";
                case Concat concat:
                    if (
                        concat.Parts.Count == 2
                        && concat.Parts[0] is LineDoc line
                        && concat.Parts[1] is BreakParent
                    )
                    {
                        return indent + (line.IsLiteral
                            ? "LiteralLine"
                            : "HardLine");
                    }

                    var result = indent + "Concat(";
                    if (concat.Parts.Count > 0)
                    {
                        result += Environment.NewLine;
                    }
                    for (var x = 0; x < concat.Parts.Count; x++)
                    {
                        var printResult = this.PrintDocTree(
                            concat.Parts[x],
                            indent + "    "
                        );
                        if (printResult == null)
                        {
                            continue;
                        }
                        result += printResult;
                        if (x < concat.Parts.Count - 1)
                        {
                            result += "," + Environment.NewLine;
                        }
                    }

                    result += ")";
                    return result;
                case LineDoc lineDoc:
                    return indent + (lineDoc.IsLiteral
                        ? "LiteralLine"
                        : lineDoc.Type == LineDoc.LineType.Normal
                            ? "Line"
                            : lineDoc.Type == LineDoc.LineType.Hard
                                ? "HardLine"
                                : "SoftLine");
                case BreakParent breakParent:
                    return "";
                case ForceFlat forceFlat:
                    return indent + "ForceFlat(" + Environment.NewLine + this.PrintDocTree(
                        forceFlat.Contents,
                        indent + "    "
                    ) + ")";
                case IndentDoc indentDoc:
                    return indent + "Indent(" + Environment.NewLine + this.PrintDocTree(
                        indentDoc.Contents,
                        indent + "    "
                    ) + ")";
                case Group group:
                    return indent + "Group(" + Environment.NewLine + this.PrintDocTree(
                        group.Contents,
                        indent + "    "
                    ) + ")";
                case LeadingComment leadingComment:
                    return indent + "LeadingComment(" + leadingComment.Comment + ", CommentType." + (leadingComment.Type == CommentType.SingleLine
                        ? "SingleLine"
                        : "MultiLine") + ")";
                case TrailingComment trailingComment:
                    return indent + "TrailingComment(" + trailingComment.Comment + ", CommentType." + (trailingComment.Type == CommentType.SingleLine
                        ? "SingleLine"
                        : "MultiLine") + ")";
                case SpaceIfNoPreviousComment spaceIfNoPreviousComment:
                    return indent + "SpaceIfNoPreviousComment";
                default:
                    throw new Exception("Can't handle " + document);
            }
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
