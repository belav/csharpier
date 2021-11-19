namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CasePatternSwitchLabel
{
    public static Doc Print(CasePatternSwitchLabelSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.Keyword, " "),
            Node.Print(node.Pattern),
            node.WhenClause != null ? Node.Print(node.WhenClause) : Doc.Null,
            Token.Print(node.ColonToken)
        );
    }
}
