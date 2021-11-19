namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MakeRefExpression
{
    public static Doc Print(MakeRefExpressionSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.Keyword),
            Token.Print(node.OpenParenToken),
            Node.Print(node.Expression),
            Token.Print(node.CloseParenToken)
        );
    }
}
