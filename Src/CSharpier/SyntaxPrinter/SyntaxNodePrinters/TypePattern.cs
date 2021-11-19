namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypePattern
{
    public static Doc Print(TypePatternSyntax node)
    {
        return Node.Print(node.Type);
    }
}
