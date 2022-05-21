namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefExpression
{
    public static Doc Print(RefExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.RefKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
