namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PostfixUnaryExpression
{
    public static Doc Print(PostfixUnaryExpressionSyntax node)
    {
        if (
            node.Kind() is SyntaxKind.SuppressNullableWarningExpression
            && node.Operand is InvocationExpressionSyntax
        )
        {
            return InvocationExpression.PrintMemberChain(node);
        }

        // TODO handle
        /*
             this.DoSomething__________________________________________________!.DoSomething__________________________________________________!.DoSomething__________________________________________________;
         */
        return Doc.Concat(Node.Print(node.Operand), Token.Print(node.OperatorToken));
    }
}
