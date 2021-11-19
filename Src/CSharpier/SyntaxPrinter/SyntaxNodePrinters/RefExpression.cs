namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefExpression
{
    public static Doc Print(RefExpressionSyntax node)
    {
        return Doc.Concat(Token.PrintWithSuffix(node.RefKeyword, " "), Node.Print(node.Expression));
    }
}
