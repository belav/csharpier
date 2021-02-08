using System;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace CSharpier
{
    public class CodeFormatter
    {
        // TODO we should make this work in parallel to speed things up, that would probably happen in CLI
        public CSharpierResult Format(string code, Options options)
        {
            var rootNode = CSharpSyntaxTree.ParseText(code).GetRoot() as CompilationUnitSyntax;

            var document = new Printer().Print(rootNode);

            return new CSharpierResult
            {
                Code = new DocPrinter().Print(document, options),
                DocTree = options.IncludeDocTree ? this.PrintDocTree(document, "") : null,
                AST = options.IncludeAST ? this.PrintAST(rootNode) : null
            };
        }
        
        private string PrintAST(CompilationUnitSyntax rootNode)
        {
            var stringBuilder = new StringBuilder();
            SyntaxNodeJsonWriter.WriteCompilationUnitSyntax(stringBuilder, rootNode);
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(stringBuilder.ToString()), Formatting.Indented);
        }

        private string PrintDocTree(Doc document, string indent)
        {
            switch (document)
            {
                case StringDoc stringDoc:
                    return indent + "\"" + stringDoc.Value + "\"";
                case Concat concat:
                    if (concat.Parts.Count == 2 && concat.Parts[0] is LineDoc line && concat.Parts[1] is BreakParent) {
                        return indent + (line.IsLiteral ? "LiteralLine" : "HardLine");
                    }
                    
                    var result = indent + "Concat(";
                    if (concat.Parts.Count > 0)
                    {
                        result += Environment.NewLine;
                    }
                    for (var x = 0; x < concat.Parts.Count; x++)
                    {
                        result += this.PrintDocTree(concat.Parts[x], indent + "    ");
                        if (x < concat.Parts.Count - 1)
                        {
                            result += "," + Environment.NewLine;
                        }
                    }

                    result += ")";
                    return result;
                case LineDoc lineDoc:
                    return indent + (lineDoc.Type == LineDoc.LineType.Normal ? "Line" : "SoftLine");
                case BreakParent breakParent:
                    return indent + "breakParent";
                case IndentDoc indentDoc:
                    return indent + "Indent(" + Environment.NewLine + this.PrintDocTree(indentDoc.Contents, indent + "    ") + ")";
                case Group group:
                    return indent + "Group(" + Environment.NewLine + this.PrintDocTree(group.Contents, indent + "    ") + ")";
                case LeadingComment leadingComment:
                    return indent + "LeadingComment(" + leadingComment.Comment + ", CommentType." + (leadingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine") + ")";
                case TrailingComment trailingComment:
                    return indent + "TrailingComment(" + trailingComment.Comment + ", CommentType." + (trailingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine") + ")";
                case SpaceIfNoPreviousComment spaceIfNoPreviousComment:
                    return indent + "SpaceIfNoPreviousComment"; 
                default:
                    throw new Exception("Can't handle " + document);
            }
        }
    }

    public class CSharpierResult
    {
        public string Code { get; set; }
        public string DocTree { get; set; }
        public string AST { get; set; }
    }
}