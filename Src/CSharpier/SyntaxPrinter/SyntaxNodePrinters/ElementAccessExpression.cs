namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementAccessExpression
{
    public static Doc Print(ElementAccessExpressionSyntax node)
    {
        return Doc.Concat(Node.Print(node.Expression), Node.Print(node.ArgumentList));
    }
}
