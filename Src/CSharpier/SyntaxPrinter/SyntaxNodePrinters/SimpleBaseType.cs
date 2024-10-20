namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SimpleBaseType
{
    public static Doc Print(SimpleBaseTypeSyntax node, PrintingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
