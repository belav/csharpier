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
                    PrintArm,
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

    private static Doc PrintArm(
        SwitchExpressionArmSyntax switchExpressionArm,
        FormattingContext context
    )
    {
        var arrowHasComment = switchExpressionArm.EqualsGreaterThanToken.LeadingTrivia.Any(o =>
            o.IsComment()
        );

        var groupId2 = Guid.NewGuid().ToString();
        var innerContents = arrowHasComment
            ? Doc.Indent(
                Doc.Concat(
                    Doc.Line,
                    Token.PrintWithSuffix(switchExpressionArm.EqualsGreaterThanToken, " ", context),
                    Node.Print(switchExpressionArm.Expression, context)
                )
            )
            : Doc.Concat(
                " ",
                Token.Print(switchExpressionArm.EqualsGreaterThanToken, context),
                Doc.GroupWithId(groupId2, Doc.Indent(Doc.Line)),
                Doc.IndentIfBreak(Node.Print(switchExpressionArm.Expression, context), groupId2)
            );

        var groupId1 = Guid.NewGuid().ToString();

        return Doc.Concat(
            ExtraNewLines.Print(switchExpressionArm),
            Token.PrintLeadingTrivia(
                switchExpressionArm.Pattern.GetLeadingTrivia(),
                context.WithSkipNextLeadingTrivia()
            ),
            Doc.Group(
                Doc.GroupWithId(
                    groupId1,
                    Doc.Concat(
                        Node.Print(switchExpressionArm.Pattern, context),
                        switchExpressionArm.WhenClause != null
                            ? Node.Print(switchExpressionArm.WhenClause, context)
                            : Doc.Null
                    )
                ),
                innerContents
            )
        );
    }
}
