namespace CSharpier.SyntaxPrinter;

internal static class SeparatedSyntaxList
{
    public static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, FormattingContext, Doc> printFunc,
        Doc afterSeparator,
        FormattingContext context,
        bool addTrailingSeparator = false,
        Doc? separator = null,
        int startingIndex = 0
    )
        where T : SyntaxNode
    {
        if (list.Count == 0)
        {
            return Doc.Null;
        }

        separator ??= (
            list.Count == 1
                ? Doc.Null
                : Token.Print(list.GetSeparator(0), context));
        
        var docs = new List<Doc>();
        for (var x = startingIndex; x < list.Count; x++)
        {
            docs.Add(printFunc(list[x], context));
            
            var isTrailingSeparator = x == list.Count - 1;
            if (isTrailingSeparator)
            {
                if (addTrailingSeparator)
                {
                    docs.Add(Doc.IfBreak(separator, Doc.Null));
                }
            }
            else
            {
                docs.Add(separator);
                docs.Add(afterSeparator);
            }
        }

        return Doc.Concat(docs);
    }
}
