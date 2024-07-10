namespace CSharpier.SyntaxPrinter;

internal static class SeparatedSyntaxList
{
    public static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, FormattingContext, Doc> printFunc,
        Doc afterSeparator,
        FormattingContext context,
        int startingIndex = 0,
        Doc? trailingSeparator = null
    )
        where T : SyntaxNode
    {
        var docs = new List<Doc>();
        for (var x = startingIndex; x < list.Count; x++)
        {
            docs.Add(printFunc(list[x], context));

            var isTrailingSeparator = x == list.Count - 1;
            SyntaxToken? trailingSeparatorToken = null;

            if (isTrailingSeparator && x < list.SeparatorCount)
            {
                trailingSeparatorToken = list.GetSeparator(x);
            }

            if (x >= list.SeparatorCount)
            {
                if (
                    (
                        !trailingSeparatorToken.HasValue
                        || !trailingSeparatorToken.Value.TrailingTrivia.Any(o => o.IsComment())
                    )
                    && isTrailingSeparator
                    && trailingSeparator != null
                )
                {
                    docs.Add(trailingSeparator);
                }

                continue;
            }

            if (isTrailingSeparator)
            {
                if (
                    trailingSeparatorToken.HasValue
                    && trailingSeparatorToken.Value.TrailingTrivia.Any(o => o.IsComment())
                )
                {
                    docs.Add(Token.Print(list.GetSeparator(x), context));
                }
                // TODO I think we can ditch this parameter, and if there is no current one, just call the TrailingComma.Print ourselves
                else if (trailingSeparator != null)
                {
                    docs.Add(trailingSeparator);
                }
                else
                {
                    docs.Add(Doc.IfBreak(Token.Print(list.GetSeparator(x), context), Doc.Null));
                }
            }
            else
            {
                docs.Add(Token.Print(list.GetSeparator(x), context));
                docs.Add(afterSeparator);
            }
        }

        return docs.Count == 0 ? Doc.Null : Doc.Concat(docs);
    }
}
