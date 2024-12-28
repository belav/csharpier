namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThisExpression
{
    public static Doc Print(ThisExpressionSyntax node, PrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
