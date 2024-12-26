namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LiteralExpression
{
    public static Doc Print(LiteralExpressionSyntax node, PrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
