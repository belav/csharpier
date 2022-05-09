namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DefaultConstraint
{
    public static Doc Print(DefaultConstraintSyntax node, FormattingContext context)
    {
        return Token.Print(node.DefaultKeyword, context);
    }
}
