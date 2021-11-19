namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AssignmentExpression
{
    public static Doc Print(AssignmentExpressionSyntax node)
    {
        return RightHandSide.Print(
            node,
            Doc.Concat(Node.Print(node.Left), " "),
            Token.Print(node.OperatorToken),
            node.Right
        );
    }
}
