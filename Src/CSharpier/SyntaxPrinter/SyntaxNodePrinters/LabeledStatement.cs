namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LabeledStatement
{
    public static Doc Print(LabeledStatementSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null]);
        docs.Append(ExtraNewLines.Print(node));
        docs.Append(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Append(Token.Print(node.Identifier, context));
        docs.Append(Token.Print(node.ColonToken, context));

        if (node.Statement is BlockSyntax blockSyntax)
        {
            docs.Append(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Append(Doc.HardLine, Node.Print(node.Statement, context));
        }
        return Doc.Concat(ref docs);
    }
}
