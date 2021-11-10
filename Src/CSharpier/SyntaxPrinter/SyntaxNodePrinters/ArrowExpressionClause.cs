using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ArrowExpressionClause
    {
        public static Doc Print(ArrowExpressionClauseSyntax node)
        {
            return Doc.Group(
                Doc.Indent(
                    " ",
                    Token.PrintWithSuffix(node.ArrowToken, Doc.Line),
                    Node.Print(node.Expression)
                )
            );
        }
    }
}
