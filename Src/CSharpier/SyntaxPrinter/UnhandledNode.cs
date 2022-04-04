namespace CSharpier.SyntaxPrinter;

internal static class UnhandledNode
{
    public static Doc Print(SyntaxNode node)
    {
#if DEBUG
        return node.GetType().FullName;
#else
        return node.ToString();
#endif
    }
}
