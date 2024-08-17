namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TryStatement
{
    public static Doc Print(TryStatementSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            AttributeLists.Print(node, node.AttributeLists, context),
            Token.Print(node.TryKeyword, context),
            Block.Print(node.Block, context),
            node.Catches.Any() ? Doc.HardLine : Doc.Null,
            Doc.Join(Doc.HardLine, node.Catches.Select(o => CatchClause.Print(o, context))),
        };
        if (node.Finally != null)
        {
            docs.Add(Doc.HardLine, FinallyClause.Print(node.Finally, context));
        }
        return Doc.Concat(docs);
    }
}
