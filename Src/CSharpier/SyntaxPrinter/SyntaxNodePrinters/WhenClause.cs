namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhenClause
{
    public static Doc Print(WhenClauseSyntax node)
    {
        var content = new[]
        {
            Doc.Line,
            Token.PrintWithSuffix(node.WhenKeyword, " "),
            Node.Print(node.Condition)
        };

        return Doc.Group(
            node.Parent is CasePatternSwitchLabelSyntax ? Doc.Concat(content) : Doc.Indent(content)
        );
    }
}
