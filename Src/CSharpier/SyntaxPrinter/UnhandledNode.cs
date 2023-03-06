namespace CSharpier.SyntaxPrinter;

internal static class UnhandledNode
{
    public static Doc Print(SyntaxNode node, FormattingContext context)
    {
        return node.ToString();
    }
}
