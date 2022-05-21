namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementBindingExpression
{
    public static Doc Print(ElementBindingExpressionSyntax node, FormattingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
