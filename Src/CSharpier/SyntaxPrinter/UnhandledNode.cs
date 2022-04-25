namespace CSharpier.SyntaxPrinter;

internal static class UnhandledNode
{
    public static Doc Print(SyntaxNode node)
    {
        if (node == null)
        {
            return Doc.Null;
        }
#if DEBUG
        return node.GetType().FullName;
#else
        return node.ToString();
#endif
    }
}
