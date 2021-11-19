namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousMethodExpression
{
    public static Doc Print(AnonymousMethodExpressionSyntax node)
    {
        var docs = new List<Doc>
        {
            Modifiers.Print(node.Modifiers),
            Token.Print(node.DelegateKeyword)
        };

        if (node.ParameterList != null)
        {
            docs.Add(ParameterList.Print(node.ParameterList));
        }

        docs.Add(Block.Print(node.Block));

        return Doc.Concat(docs);
    }
}
