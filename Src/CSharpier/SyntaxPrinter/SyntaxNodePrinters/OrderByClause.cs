namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OrderByClause
{
    public static Doc Print(OrderByClauseSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.OrderByKeyword),
            SeparatedSyntaxList.Print(
                node.Orderings,
                orderingNode =>
                    Doc.Concat(
                        " ",
                        Node.Print(orderingNode.Expression),
                        string.IsNullOrEmpty(orderingNode.AscendingOrDescendingKeyword.Text)
                          ? Doc.Null
                          : " ",
                        Token.Print(orderingNode.AscendingOrDescendingKeyword)
                    ),
                Doc.Null
            )
        );
    }
}
