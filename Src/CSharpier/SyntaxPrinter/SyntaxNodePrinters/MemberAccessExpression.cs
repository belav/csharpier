namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberAccessExpression
{
    public static Doc Print(MemberAccessExpressionSyntax node, FormattingContext context)
    {
        return InvocationExpression.PrintMemberChain(node, context);
    }
}
