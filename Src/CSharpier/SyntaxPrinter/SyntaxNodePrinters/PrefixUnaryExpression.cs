namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PrefixUnaryExpression
{
    public static Doc Print(PrefixUnaryExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.OperatorToken, context),
            Node.Print(node.Operand, context)
        );
    }
}
