namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NamespaceDeclaration
{
    public static Doc Print(NamespaceDeclarationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.Print(node.Modifiers, context),
            Token.Print(node.NamespaceKeyword, context),
            " ",
            Node.Print(node.Name, context),
        };

        var innerDocs = new List<Doc>();
        var hasMembers = node.Members.Count > 0;
        var hasUsing = node.Usings.Count > 0;
        var hasExterns = node.Externs.Count > 0;
        if (hasMembers || hasUsing || hasExterns)
        {
            innerDocs.Add(Doc.HardLine);
            NamespaceLikePrinter.Print(node, innerDocs, context);
        }
        else
        {
            innerDocs.Add(" ");
        }

        DocUtilities.RemoveInitialDoubleHardLine(innerDocs);

        docs.Add(
            Doc.Group(
                Doc.Line,
                Token.Print(node.OpenBraceToken, context),
                Doc.Indent(innerDocs),
                hasMembers || hasUsing || hasExterns ? Doc.HardLine : Doc.Null,
                Token.Print(node.CloseBraceToken, context),
                Token.Print(node.SemicolonToken, context)
            )
        );
        return Doc.Concat(docs);
    }
}
