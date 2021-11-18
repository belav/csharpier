using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefTypeExpression
{
    public static Doc Print(RefTypeExpressionSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.Keyword),
            Token.Print(node.OpenParenToken),
            Node.Print(node.Expression),
            Token.Print(node.CloseParenToken)
        );
    }
}
