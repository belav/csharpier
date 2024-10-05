namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypePattern
{
    public static Doc Print(TypePatternSyntax node, PrintingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
