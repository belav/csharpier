namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PostfixUnaryExpression
{
    public static Doc Print(PostfixUnaryExpressionSyntax node)
    {
        if (
            node.IsKind(SyntaxKind.SuppressNullableWarningExpression)
            && node.Operand is InvocationExpressionSyntax
        )
        {
            return InvocationExpression.PrintMemberChain(node);
        }

        return Doc.Concat(Node.Print(node.Operand), Token.Print(node.OperatorToken));
    }
}
