namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleExpression
{
    public static Doc Print(TupleExpressionSyntax node, FormattingContext context) =>
        Doc.Group(
            ArgumentListLike.Print(node.OpenParenToken, node.Arguments, node.CloseParenToken, context)
        );
}
