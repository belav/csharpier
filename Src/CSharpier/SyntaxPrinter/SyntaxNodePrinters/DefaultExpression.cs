namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DefaultExpression
{
    public static Doc Print(DefaultExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Type, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
