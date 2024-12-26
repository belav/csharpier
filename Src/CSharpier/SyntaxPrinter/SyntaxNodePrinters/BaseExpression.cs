namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseExpression
{
    public static Doc Print(BaseExpressionSyntax node, PrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
