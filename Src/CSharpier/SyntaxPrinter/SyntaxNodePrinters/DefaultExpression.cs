using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DefaultExpression
    {
        public static Doc Print(DefaultExpressionSyntax node)
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
