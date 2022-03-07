namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberAccessExpression
{
    public static Doc Print(MemberAccessExpressionSyntax node)
    {
        return InvocationExpression.PrintMemberChain(node);
    }
}
