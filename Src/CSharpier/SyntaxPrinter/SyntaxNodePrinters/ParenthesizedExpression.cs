using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ParenthesizedExpression
    {
        public static Doc Print(ParenthesizedExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
