namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ListPattern
{
    public static Doc Print(ListPatternSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenBracketToken, context),
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.PrintWithTrailingComma(
                    node.Patterns,
                    Node.Print,
                    Doc.Line,
                    context,
                    node.CloseBracketToken
                )
            ),
            Doc.SoftLine,
            Token.Print(node.CloseBracketToken, context),
            node.Designation is not null ? " " : Doc.Null,
            Node.Print(node.Designation, context)
        );
    }
}
