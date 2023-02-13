namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LetClause
{
    public static Doc Print(LetClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.LetKeyword, " ", context),
            Token.PrintWithSuffix(node.Identifier, " ", context),
            Token.PrintWithSuffix(node.EqualsToken, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
