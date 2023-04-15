namespace CSharpier.SyntaxPrinter;

internal static class UnhandledNode
{
    public static Doc Print(SyntaxNode node, FormattingContext context)
    {
        // full string includes comments/directives but also any whitespace, which we need to strip
        return node.ToFullString().Trim();
    }
}
