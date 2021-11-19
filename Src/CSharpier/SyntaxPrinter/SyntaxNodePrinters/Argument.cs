namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Argument
{
    public static Doc Print(ArgumentSyntax node)
    {
        var docs = new List<Doc>();
        if (node.NameColon != null)
        {
            docs.Add(BaseExpressionColon.Print(node.NameColon));
        }

        if (node.RefKindKeyword.Kind() != SyntaxKind.None)
        {
            docs.Add(Token.PrintWithSuffix(node.RefKindKeyword, " "));
        }

        docs.Add(Node.Print(node.Expression));
        return Doc.Concat(docs);
    }
}
