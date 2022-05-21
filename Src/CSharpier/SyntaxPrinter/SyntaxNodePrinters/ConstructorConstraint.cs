namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstructorConstraint
{
    public static Doc Print(ConstructorConstraintSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.NewKeyword, context),
            Token.Print(node.OpenParenToken, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
