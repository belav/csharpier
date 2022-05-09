namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AwaitExpression
{
    public static Doc Print(AwaitExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.AwaitKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
