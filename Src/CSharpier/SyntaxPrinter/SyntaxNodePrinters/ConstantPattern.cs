namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstantPattern
{
    public static Doc Print(ConstantPatternSyntax node)
    {
        return Node.Print(node.Expression);
    }
}
