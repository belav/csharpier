namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhenClause
{
    public static Doc Print(WhenClauseSyntax node)
    {
        return Doc.Group(
            Doc.Indent(
                Doc.Line,
                Token.PrintWithSuffix(node.WhenKeyword, " "),
                Node.Print(node.Condition)
            )
        );
    }
}
