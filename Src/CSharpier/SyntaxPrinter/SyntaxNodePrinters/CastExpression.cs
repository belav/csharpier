namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CastExpression
{
    public static Doc Print(CastExpressionSyntax node, PrintingContext context)
    {
        return
            node.Expression is ParenthesizedExpressionSyntax
            || (
                node.Expression
                    is IdentifierNameSyntax
                        or InvocationExpressionSyntax { Expression: IdentifierNameSyntax }
                && node.Type is not GenericNameSyntax
            )
            ? Doc.Concat(
                Token.Print(node.OpenParenToken, context),
                Node.Print(node.Type, context),
                Token.Print(node.CloseParenToken, context),
                Node.Print(node.Expression, context)
            )
            : Doc.Group(
                Token.Print(node.OpenParenToken, context),
                Node.Print(node.Type, context),
                Token.Print(node.CloseParenToken, context),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression, context))
            );
    }
}
