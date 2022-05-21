namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WithExpression
{
    public static Doc Print(WithExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            " ",
            Token.PrintWithSuffix(node.WithKeyword, Doc.Line, context),
            Node.Print(node.Initializer, context)
        );
    }
}
