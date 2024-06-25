namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryBody
{
    public static Doc Print(QueryBodySyntax node, FormattingContext context)
    {
        Doc clausesSeparator = (context.NewLineBetweenQueryExpressionClauses == true) && (node.Clauses.Count > 0)
            ? Doc.HardLine
            : (context.NewLineBetweenQueryExpressionClauses == false ? " " : Doc.Line);
        var docs = new List<Doc>
        {
            clausesSeparator,
            Doc.Join(clausesSeparator, node.Clauses.Select(o => Node.Print(o, context)))
        };
        if (node.Clauses.Count > 0)
        {
            docs.Add(Doc.Line);
        }

        docs.Add(Node.Print(node.SelectOrGroup, context));
        if (node.Continuation != null)
        {
            docs.Add(" ", QueryContinuation.Print(node.Continuation, context));
        }

        return Doc.Concat(docs);
    }
}
