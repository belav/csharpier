namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CastExpression
{
    public static Doc Print(CastExpressionSyntax node)
    {
        return
            node.Expression
                is ParenthesizedExpressionSyntax
                    or IdentifierNameSyntax
                    or InvocationExpressionSyntax { Expression: IdentifierNameSyntax }
          ? Doc.Concat(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken),
                Node.Print(node.Expression)
            )
          : Doc.Group(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Type),
                Token.Print(node.CloseParenToken),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression))
            );
    }
}
