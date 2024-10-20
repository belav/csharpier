namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementBindingExpression
{
    public static Doc Print(ElementBindingExpressionSyntax node, PrintingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
