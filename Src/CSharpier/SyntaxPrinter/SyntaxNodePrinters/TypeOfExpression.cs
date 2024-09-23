namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeOfExpression
{
    public static Doc Print(TypeOfExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Type, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
