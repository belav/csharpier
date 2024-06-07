namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConditionalExpression
{
    // TODO a decent amount of edgecases to figure out in
    // https://github.com/belav/csharpier-repos/pull/110/files
    public static Doc Print(ConditionalExpressionSyntax node, FormattingContext context)
    {
        if (
            node.WhenTrue is not ConditionalExpressionSyntax
            && node.WhenFalse is ConditionalExpressionSyntax
        )
        {
            return DoNewThing(node, context);
        }
        else
        {
            return DoOldThing(node, context);
        }
    }

    // TODO get better names on this
    public static Doc DoOldThing(ConditionalExpressionSyntax node, FormattingContext context)
    {
        var whenTrue = node.WhenTrue is ConditionalExpressionSyntax c1
            ? DoOldThing(c1, context)
            : Node.Print(node.WhenTrue, context);

        var whenFalse = node.WhenFalse is ConditionalExpressionSyntax c2
            ? DoOldThing(c2, context)
            : Node.Print(node.WhenFalse, context);

        Doc[] innerContents =
        [
            Doc.Line,
            Token.PrintWithSuffix(node.QuestionToken, " ", context),
            Doc.Concat(whenTrue),
            Doc.Line,
            Token.PrintWithSuffix(node.ColonToken, " ", context),
            Doc.Concat(whenFalse)
        ];

        Doc[] outerContents =
        [
            node.Parent is ConditionalExpressionSyntax ? Doc.BreakParent : Doc.Null,
            node
                is {
                    Parent: ReturnStatementSyntax,
                    Condition: BinaryExpressionSyntax or IsPatternExpressionSyntax
                }
                ? Doc.Indent(
                    Doc.Group(
                        Doc.IfBreak(Doc.SoftLine, Doc.Null),
                        Node.Print(node.Condition, context)
                    )
                )
                : Node.Print(node.Condition, context),
            node.Parent is ConditionalExpressionSyntax or ArgumentSyntax or ReturnStatementSyntax
            || node.Condition is InvocationExpressionSyntax
                ? Doc.Indent(innerContents)
                : Doc.Indent(innerContents)
        ];

        return node.Parent is ConditionalExpressionSyntax
            ? Doc.Concat(outerContents)
            : Doc.Group(outerContents);
    }

    private static Doc DoNewThing(ConditionalExpressionSyntax node, FormattingContext context)
    {
        var whenTrue = node.WhenTrue is ConditionalExpressionSyntax c1
            ? DoNewThing(c1, context)
            : Node.Print(node.WhenTrue, context);

        var whenFalse = node.WhenFalse is ConditionalExpressionSyntax c2
            ? DoNewThing(c2, context)
            : Node.Print(node.WhenFalse, context);

        var contents = Doc.Group(
            node.Parent is ConditionalExpressionSyntax ? Doc.BreakParent : Doc.Null,
            Doc.Group(
                Node.Print(node.Condition, context),
                Doc.Line,
                Token.PrintWithSuffix(node.QuestionToken, " ", context),
                Doc.IndentIf(node.WhenTrue is ConditionalExpressionSyntax, whenTrue)
            ),
            Doc.Line,
            Doc.Group(Token.PrintWithSuffix(node.ColonToken, " ", context), whenFalse)
        );

        if (node.Parent is ReturnStatementSyntax)
        {
            return Doc.Indent(contents);
        }

        return contents;
    }
}
