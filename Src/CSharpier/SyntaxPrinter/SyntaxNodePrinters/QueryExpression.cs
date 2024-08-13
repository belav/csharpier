namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryExpression
{
    public static Doc Print(QueryExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            FromClause.Print(node.FromClause, context),
            QueryBody.Print(node.Body, context)
        );
    }
}
