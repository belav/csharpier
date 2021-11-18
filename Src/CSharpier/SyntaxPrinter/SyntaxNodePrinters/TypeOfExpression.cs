namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeOfExpression
{
    public static Doc Print(TypeOfExpressionSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.Keyword),
            Token.Print(node.OpenParenToken),
            Node.Print(node.Type),
            Token.Print(node.CloseParenToken)
        );
    }
}
