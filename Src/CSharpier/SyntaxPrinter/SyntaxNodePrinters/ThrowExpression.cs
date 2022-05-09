namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThrowExpression
{
    public static Doc Print(ThrowExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.ThrowKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
