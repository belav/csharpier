namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OrderByClause
{
    public static Doc Print(OrderByClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.OrderByKeyword, context),
            SeparatedSyntaxList.Print(
                node.Orderings,
                (orderingNode, _) =>
                    Doc.Concat(
                        " ",
                        Node.Print(orderingNode.Expression, context),
                        string.IsNullOrEmpty(orderingNode.AscendingOrDescendingKeyword.Text)
                            ? Doc.Null
                            : " ",
                        Token.Print(orderingNode.AscendingOrDescendingKeyword, context)
                    ),
                Doc.Null,
                context
            )
        );
    }
}
