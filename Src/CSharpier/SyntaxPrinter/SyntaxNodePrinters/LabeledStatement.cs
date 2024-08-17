namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LabeledStatement
{
    public static Doc Print(LabeledStatementSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            AttributeLists.Print(node, node.AttributeLists, context),
            Token.Print(node.Identifier, context),
            Token.Print(node.ColonToken, context),
        };
        if (node.Statement is BlockSyntax blockSyntax)
        {
            docs.Add(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Add(Doc.HardLine, Node.Print(node.Statement, context));
        }
        return Doc.Concat(docs);
    }
}
