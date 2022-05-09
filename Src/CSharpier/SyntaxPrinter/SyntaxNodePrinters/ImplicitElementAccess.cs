namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitElementAccess
{
    public static Doc Print(ImplicitElementAccessSyntax node, FormattingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
