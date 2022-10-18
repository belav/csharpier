namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AwaitExpression
{
    public static Doc Print(AwaitExpressionSyntax node, FormattingContext context)
    {
        var precedesQueryExpression = node.Expression is QueryExpressionSyntax;

        return Doc.Concat(
            Token.PrintWithSuffix(node.AwaitKeyword, " ", context),
            precedesQueryExpression
                ? Doc.Indent(Doc.Line, Node.Print(node.Expression, context))
                : Node.Print(node.Expression, context)
        );
    }
}
