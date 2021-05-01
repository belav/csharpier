using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ThrowExpression
    {
        public static Doc Print(ThrowExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.ThrowKeyword, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
