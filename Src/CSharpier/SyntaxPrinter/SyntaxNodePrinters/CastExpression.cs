namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CastExpression
{
    public static Doc Print(CastExpressionSyntax node)
    {
        // TODO review https://github.com/belav/csharpier-repos/pull/37/files for edge cases
        return node.Expression is ParenthesizedExpressionSyntax parenthesizedExpression
          ? Doc.Concat(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken),
                ParenthesizedExpression.Print(parenthesizedExpression)
            )
          : Doc.Group(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression))
            );
    }
}
