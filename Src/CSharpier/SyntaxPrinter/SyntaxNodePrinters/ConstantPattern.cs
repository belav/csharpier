namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstantPattern
{
    public static Doc Print(ConstantPatternSyntax node, FormattingContext context)
    {
        return Node.Print(node.Expression, context);
    }
}
