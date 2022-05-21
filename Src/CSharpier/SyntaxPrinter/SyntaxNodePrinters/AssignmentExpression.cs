namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AssignmentExpression
{
    public static Doc Print(AssignmentExpressionSyntax node, FormattingContext context)
    {
        return RightHandSide.Print(
            node,
            Doc.Concat(Node.Print(node.Left, context), " "),
            Token.Print(node.OperatorToken, context),
            node.Right,
            context
        );
    }
}
