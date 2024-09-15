namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchSection
{
    public static Doc Print(SwitchSectionSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            Doc.Join(Doc.HardLine, node.Labels.Select(o => Node.Print(o, context))),
        };
        if (node.Statements is [BlockSyntax blockSyntax])
        {
            docs.Add(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Add(
                Doc.Indent(
                    node.Statements.First() is BlockSyntax ? Doc.Null : Doc.HardLine,
                    Doc.Join(
                        Doc.HardLine,
                        node.Statements.Select(o => Node.Print(o, context)).ToArray()
                    )
                )
            );
        }
        return Doc.Concat(docs);
    }
}
