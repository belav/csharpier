namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitArrayCreationExpression
{
    public static Doc Print(ImplicitArrayCreationExpressionSyntax node, FormattingContext context)
    {
        var commas = node.Commas.Select(o => Token.Print(o, context)).ToArray();
        return Doc.Group(
            Token.Print(node.NewKeyword, context),
            Token.Print(node.OpenBracketToken, context),
            Doc.Concat(commas),
            Token.Print(node.CloseBracketToken, context),
            " ",
            InitializerExpression.Print(node.Initializer, context)
        );
    }
}
