namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseFieldDeclaration
{
    public static Doc Print(BaseFieldDeclarationSyntax node)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers)
        };
        if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
        {
            docs.Add(Token.PrintWithSuffix(eventFieldDeclarationSyntax.EventKeyword, " "));
        }

        docs.Add(VariableDeclaration.Print(node.Declaration), Token.Print(node.SemicolonToken));
        return Doc.Concat(docs);
    }
}
