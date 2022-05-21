namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConditionalExpression
{
    public static Doc Print(ConditionalExpressionSyntax node, FormattingContext context)
    {
        Doc[] innerContents =
        {
            Doc.Line,
            Token.PrintWithSuffix(node.QuestionToken, " ", context),
            Doc.Concat(Node.Print(node.WhenTrue, context)),
            Doc.Line,
            Token.PrintWithSuffix(node.ColonToken, " ", context),
            Doc.Concat(Node.Print(node.WhenFalse, context))
        };

        Doc[] outerContents =
        {
            node.Parent is ConditionalExpressionSyntax ? Doc.BreakParent : Doc.Null,
            node.Parent is ReturnStatementSyntax
            && node.Condition is BinaryExpressionSyntax or IsPatternExpressionSyntax
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
        };

        return node.Parent is ConditionalExpressionSyntax
          ? Doc.Concat(outerContents)
          : Doc.Group(outerContents);
    }
}
