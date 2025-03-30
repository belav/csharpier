namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Parameter
{
    public static Doc Print(ParameterSyntax node, PrintingContext context)
    {
        var hasAttribute = node.AttributeLists.Any();
        var docs = new List<Doc>();

        if (hasAttribute)
        {
            docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
            if (
                node.AttributeLists.Count < 2
                && (
                    node.GetLeadingTrivia().Any(o => o.IsComment())
                    || node.Parent is ParameterListSyntax { Parameters.Count: 0 }
                )
            )
            {
                docs.Add(" ");
            }
            else
            {
                docs.Add(Doc.Indent(Doc.Line));
            }
        }

        if (node.Modifiers.Any())
        {
            docs.Add(Modifiers.Print(node.Modifiers, context));
        }

        if (node.Type != null)
        {
            docs.Add(Node.Print(node.Type, context), " ");
        }

        docs.Add(Token.Print(node.Identifier, context));
        if (node.Default != null)
        {
            docs.Add(EqualsValueClause.Print(node.Default, context));
        }

        return hasAttribute ? Doc.Group(docs)
            : docs.Count == 1 ? docs[0]
            : Doc.Concat(docs);
    }
}
