namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Parameter
{
    public static Doc Print(ParameterSyntax node, FormattingContext context)
    {
        var hasAttribute = node.AttributeLists.Any();
        var groupId = hasAttribute ? Guid.NewGuid().ToString() : string.Empty;
        var docs = new List<Doc>();

        if (hasAttribute)
        {
            docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
            docs.Add(Doc.IndentIfBreak(Doc.Line, groupId));
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

        return hasAttribute ? Doc.GroupWithId(groupId, docs)
            : docs.Count == 1 ? docs[0]
            : Doc.Concat(docs);
    }
}
