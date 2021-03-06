using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            var syntaxTree = CSharpSyntaxTree.ParseText(
                code,
                new CSharpParseOptions(
                    LanguageVersion.CSharp9,
                    DocumentationMode.Diagnose));
            var rootNode = syntaxTree.GetRoot() as CompilationUnitSyntax;
            var diagnostics = syntaxTree.GetDiagnostics().Where(
                o => o.Severity == DiagnosticSeverity.Error &&
                     o.Id != "CS1029").ToList();
            // TODO 1 report that didn't format because of errors?
            if (diagnostics.Any())
            {
                return new CSharpierResult
                {
                    Code = code,
                    Errors = diagnostics,
                    AST = options.IncludeAST ? this.PrintAST(rootNode) : null
                };
            }

            try
            {
                var document = new Printer().Print(rootNode);

                var formattedCode = new DocPrinter().Print(document, options);
                return new CSharpierResult
                {
                    Code = formattedCode,
                    DocTree = options.IncludeDocTree
                        ? this.PrintDocTree(document, "")
                        : null,
                    AST = options.IncludeAST ? this.PrintAST(rootNode) : null
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
                rootNode);
            return JsonConvert.SerializeObject(
                JsonConvert.DeserializeObject(stringBuilder.ToString()),
                Formatting.Indented);
        }

        private string PrintDocTree(Doc document, string indent)
        {
            if (document == null)
            {
                return indent + "null";
            }
            switch (document)
            {
                case StringDoc stringDoc:
                    return indent + "\"" + stringDoc.Value + "\"";
                case Concat concat:
                    if (
                        concat.Parts.Count == 2 &&
                        concat.Parts[0] is LineDoc line &&
                        concat.Parts[1] is BreakParent
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
                            indent + "    ");
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
                    return indent + (
                        lineDoc.IsLiteral ? "LiteralLine"
                        : lineDoc.Type == LineDoc.LineType.Normal ? "Line"
                        : lineDoc.Type == LineDoc.LineType.Hard ? "HardLine"
                        : "SoftLine");
                case BreakParent breakParent:
                    return null;
                case ForceFlat forceFlat:
                    return indent + "ForceFlat(" + Environment.NewLine + this.PrintDocTree(
                        forceFlat.Contents,
                        indent + "    ") + ")";
                case IndentDoc indentDoc:
                    return indent + "Indent(" + Environment.NewLine + this.PrintDocTree(
                        indentDoc.Contents,
                        indent + "    ") + ")";
                case Group group:
                    return indent + "Group(" + Environment.NewLine + this.PrintDocTree(
                        group.Contents,
                        indent + "    ") + ")";
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

        // this isn't super useful because it just shows me how to convert old into new, but doesn't show what that will actually change
        public static bool IsCodeEqualAccordingToSyntaxDiffer(
            string code,
            string formattedCode)
        {
            var type = typeof(SyntaxNode).Assembly.GetTypes().First(
                o => o.Name == "SyntaxDiffer");
            var methods = type.GetMethods(
                BindingFlags.NonPublic |
                BindingFlags.Static).Where(o => o.Name == "GetTextChanges");
            foreach (var method in methods)
            {
                var blah = method.GetParameters();
                if (blah.Length == 2 && blah[0].ParameterType == typeof(SyntaxTree))
                {
                    var result = method.Invoke(
                        null,
                        new[] {
                            CSharpSyntaxTree.ParseText(code),
                            CSharpSyntaxTree.ParseText(formattedCode)
                        }) as IList<TextChange>;
                    return result.Count == 0;
                }
            }

            return false;
        }
    }

    public class CSharpierResult
    {
        public string Code { get; set; }
        public string DocTree { get; set; }
        public string AST { get; set; }
        public bool TestRunFailed { get; set; }
        public IEnumerable<Diagnostic> Errors { get; set; } = Enumerable.Empty<Diagnostic>();
        public string FailureMessage { get; set; }
    }
}
