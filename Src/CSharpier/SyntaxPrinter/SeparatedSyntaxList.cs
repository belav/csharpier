namespace CSharpier.SyntaxPrinter;

internal static class SeparatedSyntaxList
{
    public static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, Doc> printFunc,
        Doc afterSeparator,
        int startingIndex = 0
    ) where T : SyntaxNode
    {
        var docs = new List<Doc>();
        for (var x = startingIndex; x < list.Count; x++)
        {
            docs.Add(printFunc(list[x]));

            if (x >= list.SeparatorCount)
            {
                continue;
            }

            var isTrailingSeparator = x == list.Count - 1;

            docs.Add(Token.Print(list.GetSeparator(x)));
            if (!isTrailingSeparator)
            {
                docs.Add(afterSeparator);
            }
        }

        return docs.Count == 0 ? Doc.Null : Doc.Concat(docs);
    }
}
