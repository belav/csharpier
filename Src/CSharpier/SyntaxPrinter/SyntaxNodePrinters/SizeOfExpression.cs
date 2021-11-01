using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class SizeOfExpression
    {
        public static Doc Print(SizeOfExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
