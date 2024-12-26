namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitElementAccess
{
    public static Doc Print(ImplicitElementAccessSyntax node, PrintingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
