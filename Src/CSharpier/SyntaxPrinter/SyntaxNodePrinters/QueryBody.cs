namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryBody
{
    public static Doc Print(QueryBodySyntax node, FormattingContext context)
    {
        var docs = new List<Doc> { Doc.Join(Doc.Line, node.Clauses.Select(o => Node.Print(o, context))) };
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
