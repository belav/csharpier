namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Argument
{
    public static Doc Print(ArgumentSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>();
        if (node.NameColon != null)
        {
            docs.Add(BaseExpressionColon.Print(node.NameColon, context));
        }

        if (node.RefKindKeyword.RawSyntaxKind() != SyntaxKind.None)
        {
            docs.Add(Token.PrintWithSuffix(node.RefKindKeyword, " ", context));
        }

        docs.Add(Node.Print(node.Expression, context));
        return Doc.Concat(docs);
    }
}
