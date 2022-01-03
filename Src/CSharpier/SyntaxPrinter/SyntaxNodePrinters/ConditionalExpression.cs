namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConditionalExpression
{
    public static Doc Print(ConditionalExpressionSyntax node)
    {
        Doc[] innerContents =
        {
            Doc.Line,
            Token.PrintWithSuffix(node.QuestionToken, " "),
            Doc.Align(2, Node.Print(node.WhenTrue)),
            Doc.Line,
            Token.PrintWithSuffix(node.ColonToken, " "),
            Doc.Align(2, Node.Print(node.WhenFalse))
        };

        Doc[] outerContents =
        {
            node.Parent is ConditionalExpressionSyntax ? Doc.BreakParent : Doc.Null,
            node.Parent is ReturnStatementSyntax
            && node.Condition is BinaryExpressionSyntax or IsPatternExpressionSyntax
                ? Doc.Indent(
                      Doc.Group(Doc.IfBreak(Doc.SoftLine, Doc.Null), Node.Print(node.Condition))
                  )
                : Node.Print(node.Condition),
            node.Parent is ConditionalExpressionSyntax or ArgumentSyntax or ReturnStatementSyntax
            || node.Condition is InvocationExpressionSyntax
                ? Doc.Align(2, innerContents)
                : Doc.Indent(innerContents)
        };

        return node.Parent is ConditionalExpressionSyntax
          ? Doc.Concat(outerContents)
          : Doc.Group(outerContents);
    }
}
