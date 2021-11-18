namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GroupClause
{
    public static Doc Print(GroupClauseSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.GroupKeyword, " "),
            Node.Print(node.GroupExpression),
            " ",
            Token.PrintWithSuffix(node.ByKeyword, " "),
            Node.Print(node.ByExpression)
        );
    }
}
