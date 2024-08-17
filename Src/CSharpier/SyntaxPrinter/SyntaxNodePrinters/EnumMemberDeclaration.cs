namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class EnumMemberDeclaration
{
    public static Doc Print(EnumMemberDeclarationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.Print(node.Modifiers, context),
            Token.Print(node.Identifier, context),
        };
        if (node.EqualsValue != null)
        {
            docs.Add(EqualsValueClause.Print(node.EqualsValue, context));
        }
        return Doc.Concat(docs);
    }
}
