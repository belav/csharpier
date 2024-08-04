namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchExpression
{
    public static Doc Print(SwitchExpressionSyntax node, FormattingContext context)
    {
        var sections = Doc.Group(
            Doc.Indent(
                Doc.HardLine,
                SeparatedSyntaxList.PrintWithTrailingComma(
                    node.Arms,
                    (o, _) =>
                        Doc.Concat(
                            ExtraNewLines.Print(o),
                            Token.PrintLeadingTrivia(
                                o.Pattern.GetLeadingTrivia(),
                                context.WithSkipNextLeadingTrivia()
                            ),
                            Doc.Group(
                                Node.Print(o.Pattern, context),
                                o.WhenClause != null ? Node.Print(o.WhenClause, context) : Doc.Null,
                                Doc.Indent(
                                    Doc.Concat(
                                        Doc.Line,
                                        Token.PrintWithSuffix(
                                            o.EqualsGreaterThanToken,
                                            " ",
                                            context
                                        ),
                                        Node.Print(o.Expression, context)
                                    )
                                )
                            )
                        ),
                    Doc.HardLine,
                    context,
                    node.CloseBraceToken
                )
            ),
            Doc.HardLine
        );

        DocUtilities.RemoveInitialDoubleHardLine(sections);

        return Doc.Concat(
            Node.Print(node.GoverningExpression, context),
            " ",
            Token.Print(node.SwitchKeyword, context),
            Doc.HardLine,
            Token.Print(node.OpenBraceToken, context),
            sections,
            Token.Print(node.CloseBraceToken, context)
        );
    }
}
