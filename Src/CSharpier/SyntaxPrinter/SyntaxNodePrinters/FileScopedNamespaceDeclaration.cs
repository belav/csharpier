namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FileScopedNamespaceDeclaration
{
    public static Doc Print(FileScopedNamespaceDeclarationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.Print(node.Modifiers, context),
            Token.Print(node.NamespaceKeyword, context),
            " ",
            Node.Print(node.Name, context),
            Token.Print(node.SemicolonToken, context),
            Doc.HardLine,
            Doc.HardLine,
        };

        NamespaceLikePrinter.Print(node, docs, context);

        return Doc.Concat(docs);
    }
}
