namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TryStatement
{
    public static Doc Print(TryStatementSyntax node)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            AttributeLists.Print(node, node.AttributeLists),
            Token.Print(node.TryKeyword),
            Block.Print(node.Block),
            node.Catches.Any() ? Doc.HardLine : Doc.Null,
            Doc.Join(Doc.HardLine, node.Catches.Select(CatchClause.Print))
        };
        if (node.Finally != null)
        {
            docs.Add(Doc.HardLine, FinallyClause.Print(node.Finally));
        }
        return Doc.Concat(docs);
    }
}
