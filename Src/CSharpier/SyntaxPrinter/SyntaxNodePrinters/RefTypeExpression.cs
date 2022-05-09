namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefTypeExpression
{
    public static Doc Print(RefTypeExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Expression, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
