using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class CastExpression
    {
        public static Doc Print(CastExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken),
                Node.Print(node.Expression)
            );
        }
    }
}
