using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class MakeRefExpression
    {
        public static Doc Print(MakeRefExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
