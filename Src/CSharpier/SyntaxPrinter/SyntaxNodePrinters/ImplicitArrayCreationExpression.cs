namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitArrayCreationExpression
{
    public static Doc Print(ImplicitArrayCreationExpressionSyntax node)
    {
        var commas = node.Commas.Select(Token.Print).ToArray();
        return Doc.Group(
            Token.Print(node.NewKeyword),
            Token.Print(node.OpenBracketToken),
            Doc.Concat(commas),
            Token.Print(node.CloseBracketToken),
            Doc.Line,
            InitializerExpression.Print(node.Initializer)
        );
    }
}
