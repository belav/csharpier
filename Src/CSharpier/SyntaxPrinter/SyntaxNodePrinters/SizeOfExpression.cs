namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SizeOfExpression
{
    public static Doc Print(SizeOfExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Type, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
