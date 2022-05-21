namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThisExpression
{
    public static Doc Print(ThisExpressionSyntax node, FormattingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
