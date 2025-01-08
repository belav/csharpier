namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CasePatternSwitchLabel
{
    public static Doc Print(CasePatternSwitchLabelSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.Keyword, " ", context),
            Node.Print(node.Pattern, context),
            node.WhenClause != null ? WhenClause.Print(node.WhenClause, context) : Doc.Null,
            Token.Print(node.ColonToken, context)
        );
    }
}
