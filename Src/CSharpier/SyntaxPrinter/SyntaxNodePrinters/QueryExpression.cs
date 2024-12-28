namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryExpression
{
    public static Doc Print(QueryExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            FromClause.Print(node.FromClause, context),
            Doc.Line,
            QueryBody.Print(node.Body, context)
        );
    }
}
