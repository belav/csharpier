using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CastExpression
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
