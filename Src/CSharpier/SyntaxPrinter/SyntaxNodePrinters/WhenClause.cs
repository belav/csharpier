namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhenClause
{
    public static Doc Print(WhenClauseSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Doc.Indent(
                Doc.Line,
                Token.PrintWithSuffix(node.WhenKeyword, " ", context),
                Node.Print(node.Condition, context)
            )
        );
    }
}
