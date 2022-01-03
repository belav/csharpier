namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NamespaceDeclaration
{
    public static Doc Print(NamespaceDeclarationSyntax node)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Token.Print(node.NamespaceKeyword),
            " ",
            Node.Print(node.Name)
        };

        var innerDocs = new List<Doc>();
        var hasMembers = node.Members.Count > 0;
        var hasUsing = node.Usings.Count > 0;
        var hasExterns = node.Externs.Count > 0;
        if (hasMembers || hasUsing || hasExterns)
        {
            innerDocs.Add(Doc.HardLine);
            NamespaceLikePrinter.Print(node, innerDocs);
        }
        else
        {
            innerDocs.Add(" ");
        }

        DocUtilities.RemoveInitialDoubleHardLine(innerDocs);

        docs.Add(
            Doc.Group(
                Doc.Line,
                Token.Print(node.OpenBraceToken),
                Doc.Indent(innerDocs),
                hasMembers || hasUsing || hasExterns ? Doc.HardLine : Doc.Null,
                Token.Print(node.CloseBraceToken),
                Token.Print(node.SemicolonToken)
            )
        );
        return Doc.Concat(docs);
    }
}
