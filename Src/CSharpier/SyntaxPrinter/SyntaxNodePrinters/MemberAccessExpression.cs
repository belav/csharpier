namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberAccessExpression
{
    public static Doc Print(MemberAccessExpressionSyntax node, PrintingContext context)
    {
        return InvocationExpression.PrintMemberChain(node, context);
    }
}
