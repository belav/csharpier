namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseFieldDeclaration
{
    public static Doc Print(BaseFieldDeclarationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.PrintSorted(node.Modifiers, context),
        };
        if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
        {
            docs.Add(Token.PrintWithSuffix(eventFieldDeclarationSyntax.EventKeyword, " ", context));
        }

        docs.Add(
            VariableDeclaration.Print(node.Declaration, context),
            Token.Print(node.SemicolonToken, context)
        );
        return Doc.Concat(docs);
    }
}
