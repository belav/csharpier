namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OrderByClause
{
    public static Doc Print(OrderByClauseSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.OrderByKeyword, " "),
            SeparatedSyntaxList.Print(
                node.Orderings,
                orderingNode =>
                    Doc.Concat(
                        Node.Print(orderingNode.Expression),
                        " ",
                        Token.Print(orderingNode.AscendingOrDescendingKeyword)
                    ),
                Doc.Null
            )
        );
    }
}
