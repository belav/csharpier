namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FileScopedNamespaceDeclaration
{
    public static Doc Print(FileScopedNamespaceDeclarationSyntax node)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Token.Print(node.NamespaceKeyword),
            " ",
            Node.Print(node.Name),
            Token.Print(node.SemicolonToken),
            Doc.HardLine
        };

        NamespaceLikePrinter.Print(node, docs);

        return Doc.Concat(docs);
    }
}
