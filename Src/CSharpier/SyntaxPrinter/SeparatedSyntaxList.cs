namespace CSharpier.SyntaxPrinter;

internal static class SeparatedSyntaxList
{
    public static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, FormattingContext, Doc> printFunc,
        Doc afterSeparator,
        FormattingContext context,
        int startingIndex = 0
    )
        where T : SyntaxNode
    {
        var docs = new List<Doc>();
        for (var x = startingIndex; x < list.Count; x++)
        {
            docs.Add(printFunc(list[x], context));

            if (x >= list.SeparatorCount)
            {
                continue;
            }

            var isTrailingSeparator = x == list.Count - 1;

            docs.Add(Token.Print(list.GetSeparator(x), context));
            if (!isTrailingSeparator)
            {
                docs.Add(afterSeparator);
            }
        }

        return docs.Count == 0 ? Doc.Null : Doc.Concat(docs);
    }
}
