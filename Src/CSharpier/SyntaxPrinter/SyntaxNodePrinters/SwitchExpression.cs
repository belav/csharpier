namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchExpression
{
    public static Doc Print(SwitchExpressionSyntax node)
    {
        var sections = Doc.Group(
            Doc.Indent(
                Doc.HardLine,
                SeparatedSyntaxList.Print(
                    node.Arms,
                    o =>
                        Doc.Concat(
                            ExtraNewLines.Print(o),
                            Doc.Group(
                                Node.Print(o.Pattern),
                                o.WhenClause != null ? Node.Print(o.WhenClause) : Doc.Null,
                                Doc.Indent(
                                    Doc.Concat(
                                        Doc.Line,
                                        Token.PrintWithSuffix(o.EqualsGreaterThanToken, " "),
                                        Node.Print(o.Expression)
                                    )
                                )
                            )
                        ),
                    Doc.HardLine
                )
            ),
            Doc.HardLine
        );

        DocUtilities.RemoveInitialDoubleHardLine(sections);

        return Doc.Concat(
            Node.Print(node.GoverningExpression),
            " ",
            Token.Print(node.SwitchKeyword),
            Doc.HardLine,
            Token.Print(node.OpenBraceToken),
            sections,
            Token.Print(node.CloseBraceToken)
        );
    }
}
