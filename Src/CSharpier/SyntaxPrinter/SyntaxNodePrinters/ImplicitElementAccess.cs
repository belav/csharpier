namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitElementAccess
{
    public static Doc Print(ImplicitElementAccessSyntax node)
    {
        return Node.Print(node.ArgumentList);
    }
}
