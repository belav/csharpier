namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExpressionElement
{
    public static Doc Print(ExpressionElementSyntax node, FormattingContext context)
    {
        return Node.Print(node.Expression, context);
    }
}
