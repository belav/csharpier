namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThisExpression
{
    public static Doc Print(ThisExpressionSyntax node)
    {
        return Token.Print(node.Token);
    }
}
