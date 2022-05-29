namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LiteralExpression
{
    public static Doc Print(LiteralExpressionSyntax node, FormattingContext context)
    {
        // TODO is this where raw string literals are??
        return Token.Print(node.Token, context);
    }
}
