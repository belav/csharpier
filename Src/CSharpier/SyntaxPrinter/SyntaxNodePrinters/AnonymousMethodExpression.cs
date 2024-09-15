namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousMethodExpression
{
    public static Doc Print(AnonymousMethodExpressionSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            Modifiers.Print(node.Modifiers, context),
            Token.Print(node.DelegateKeyword, context),
        };

        if (node.ParameterList != null)
        {
            docs.Add(ParameterList.Print(node.ParameterList, context));
        }

        docs.Add(Block.Print(node.Block, context));

        return Doc.Concat(docs);
    }
}
