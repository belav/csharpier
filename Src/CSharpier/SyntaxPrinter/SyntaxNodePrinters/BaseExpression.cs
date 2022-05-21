namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseExpression
{
    public static Doc Print(BaseExpressionSyntax node, FormattingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
