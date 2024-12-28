namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousObjectMemberDeclarator
{
    public static Doc Print(AnonymousObjectMemberDeclaratorSyntax node, PrintingContext context)
    {
        var docs = new List<Doc>();
        if (
            node.Parent is AnonymousObjectCreationExpressionSyntax parent
            && node != parent.Initializers.First()
        )
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        if (node.NameEquals != null)
        {
            docs.Add(Token.PrintWithSuffix(node.NameEquals.Name.Identifier, " ", context));
            docs.Add(Token.PrintWithSuffix(node.NameEquals.EqualsToken, " ", context));
        }
        docs.Add(Node.Print(node.Expression, context));
        return Doc.Concat(docs);
    }
}
