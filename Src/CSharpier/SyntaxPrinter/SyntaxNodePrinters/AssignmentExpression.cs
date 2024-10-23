namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AssignmentExpression
{
    public static Doc Print(AssignmentExpressionSyntax node, PrintingContext context)
    {
        Doc possibleNewLines = Doc.Null;

        if (node.Parent is InitializerExpressionSyntax parent && node != parent.Expressions.First())
        {
            possibleNewLines = ExtraNewLines.Print(node);
        }

        return RightHandSide.Print(
            node,
            Doc.Concat(possibleNewLines, Node.Print(node.Left, context), " "),
            Token.Print(node.OperatorToken, context),
            node.Right,
            context
        );
    }
}
