namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CasePatternSwitchLabel
{
    public static Doc Print(CasePatternSwitchLabelSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.Keyword, " "),
            Node.Print(node.Pattern),
            node.WhenClause != null ? WhenClause.Print(node.WhenClause) : Doc.Null,
            Token.Print(node.ColonToken)
        );
    }
}
