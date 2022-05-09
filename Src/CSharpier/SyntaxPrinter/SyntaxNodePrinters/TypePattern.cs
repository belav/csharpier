namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypePattern
{
    public static Doc Print(TypePatternSyntax node, FormattingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
