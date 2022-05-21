namespace CSharpier.SyntaxPrinter;

internal static class AttributeLists
{
    public static Doc Print(
        SyntaxNode node,
        SyntaxList<AttributeListSyntax> attributeLists,
        FormattingContext context
    )
    {
        if (attributeLists.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        Doc separator = node
            is TypeParameterSyntax
                or ParameterSyntax
                or ParenthesizedLambdaExpressionSyntax
            ? Doc.Line
            : Doc.HardLine;
        docs.Add(Doc.Join(separator, attributeLists.Select(o => AttributeList.Print(o, context))));

        if (node is not (ParameterSyntax or TypeParameterSyntax))
        {
            docs.Add(separator);
        }

        return Doc.Concat(docs);
    }
}
