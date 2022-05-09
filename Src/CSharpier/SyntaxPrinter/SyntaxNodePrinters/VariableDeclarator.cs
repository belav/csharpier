namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class VariableDeclarator
{
    public static Doc Print(VariableDeclaratorSyntax node, FormattingContext context)
    {
        var docs = new List<Doc> { Token.Print(node.Identifier, context) };
        if (node.ArgumentList != null)
        {
            docs.Add(BracketedArgumentList.Print(node.ArgumentList, context));
        }
        if (node.Initializer != null)
        {
            docs.Add(EqualsValueClause.Print(node.Initializer, context));
        }
        return Doc.Concat(docs);
    }
}
