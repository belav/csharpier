using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VariableDeclaration
    {
        public static Doc Print(VariableDeclarationSyntax node)
        {
            var docs = Doc.Concat(
                SeparatedSyntaxList.Print(
                    node.Variables,
                    VariableDeclarator.Print,
                    node.Parent is ForStatementSyntax ? Doc.Line : Doc.HardLine
                )
            );

            return Doc.Concat(
                Node.Print(node.Type),
                " ",
                node.Variables.Count > 1 ? Doc.Indent(docs) : docs
            );
        }
    }
}
