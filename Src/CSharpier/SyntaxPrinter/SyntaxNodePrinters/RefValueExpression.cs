using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RefValueExpression
    {
        public static Doc Print(RefValueExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Expression),
                Token.PrintWithSuffix(node.Comma, " "),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
