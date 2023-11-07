namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RecursivePattern
{
    public static Doc PrintWithOutType(RecursivePatternSyntax node, FormattingContext context)
    {
        return Print(node, false, context);
    }

    public static Doc Print(RecursivePatternSyntax node, FormattingContext context)
    {
        return Print(node, true, context);
    }

    private static Doc Print(
        RecursivePatternSyntax node,
        bool includeType,
        FormattingContext context
    )
    {
        var result = new List<Doc>();
        if (node.Type != null && includeType)
        {
            result.Add(Node.Print(node.Type, context));
        }

        if (node.PositionalPatternClause != null)
        {
            result.Add(
                node.Parent is SwitchExpressionArmSyntax or CasePatternSwitchLabelSyntax
                    ? Doc.Null
                    : Doc.SoftLine,
                Token.PrintLeadingTrivia(node.PositionalPatternClause.OpenParenToken, context),
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(
                        node.PositionalPatternClause.OpenParenToken,
                        context
                    ),
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(
                            node.PositionalPatternClause.Subpatterns,
                            (subpatternNode, _) =>
                                Doc.Concat(
                                    subpatternNode.NameColon != null
                                        ? BaseExpressionColon.Print(
                                            subpatternNode.NameColon,
                                            context
                                        )
                                        : Doc.Null,
                                    Node.Print(subpatternNode.Pattern, context)
                                ),
                            Doc.Line,
                            context
                        )
                    ),
                    Doc.SoftLine,
                    Token.Print(node.PositionalPatternClause.CloseParenToken, context)
                )
            );
        }

        if (node.PropertyPatternClause != null)
        {
            if (!node.PropertyPatternClause.Subpatterns.Any())
            {
                if (node.Type != null)
                {
                    result.Add(" ");
                }
                result.Add("{ }");
            }
            else
            {
                result.Add(
                    Token.PrintLeadingTrivia(node.PropertyPatternClause.OpenBraceToken, context),
                    Doc.Group(
                        node.Type != null
                        && !node.PropertyPatternClause
                            .OpenBraceToken
                            .LeadingTrivia
                            .Any(o => o.IsDirective || o.IsComment())
                            ? Doc.Line
                            : Doc.Null,
                        Token.PrintWithoutLeadingTrivia(
                            node.PropertyPatternClause.OpenBraceToken,
                            context
                        ),
                        Doc.Indent(
                            node.PropertyPatternClause.Subpatterns.Any() ? Doc.Line : Doc.Null,
                            SeparatedSyntaxList.Print(
                                node.PropertyPatternClause.Subpatterns,
                                (subpatternNode, _) =>
                                    Doc.Group(
                                        subpatternNode.ExpressionColon != null
                                            ? Node.Print(subpatternNode.ExpressionColon, context)
                                            : Doc.Null,
                                        Node.Print(subpatternNode.Pattern, context)
                                    ),
                                Doc.Line,
                                context
                            )
                        ),
                        Doc.Line,
                        Token.Print(node.PropertyPatternClause.CloseBraceToken, context)
                    )
                );
            }
        }

        if (node.Designation != null)
        {
            result.Add(" ", Node.Print(node.Designation, context));
        }

        return Doc.Concat(result);
    }
}
