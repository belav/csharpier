namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementAccessExpression
{
    public static Doc Print(ElementAccessExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            Node.Print(node.ArgumentList, context)
        );
    }
}
