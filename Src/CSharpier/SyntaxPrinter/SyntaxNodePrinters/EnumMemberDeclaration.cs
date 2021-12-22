namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class EnumMemberDeclaration
{
    public static Doc Print(EnumMemberDeclarationSyntax node)
    {
        var docs = new List<Doc>
        {
            node.GetLeadingTrivia()
            .Any(
                o =>
                    o.Kind()
                        is SyntaxKind.SingleLineCommentTrivia
                            or SyntaxKind.MultiLineCommentTrivia
                            or SyntaxKind.SingleLineDocumentationCommentTrivia
                            or SyntaxKind.MultiLineDocumentationCommentTrivia
            )
                ? ExtraNewLines.Print(node)
                : Doc.Null,
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Token.Print(node.Identifier)
        };
        if (node.EqualsValue != null)
        {
            docs.Add(EqualsValueClause.Print(node.EqualsValue));
        }
        return Doc.Concat(docs);
    }
}
