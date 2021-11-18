namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryExpression
{
    public static Doc Print(QueryExpressionSyntax node)
    {
        return Doc.Concat(FromClause.Print(node.FromClause), Doc.Line, QueryBody.Print(node.Body));
    }
}
