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
        return Print(list, printFunc, afterSeparator, context, startingIndex, null);
    }

    public static Doc PrintWithTrailingComma<T>(
        SeparatedSyntaxList<T> list,
        Func<T, FormattingContext, Doc> printFunc,
        Doc afterSeparator,
        FormattingContext context,
        SyntaxToken? closingToken = null
    )
        where T : SyntaxNode
    {
        return Print(list, printFunc, afterSeparator, context, 0, closingToken);
    }

    // the names above aren't totally accurate
    // sometimes there are trailing commas with calls to Print (some patterns do that)
    // and if you pass null to PrintWithTrailingComma it won't add a trailing comma if there isn't one
    private static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, FormattingContext, Doc> printFunc,
        Doc afterSeparator,
        FormattingContext context,
        int startingIndex,
        SyntaxToken? closingToken
    )
        where T : SyntaxNode
    {
        var docs = new List<Doc>();
        for (var x = startingIndex; x < list.Count; x++)
        {
            docs.Add(printFunc(list[x], context));

            // if the syntax tree doesn't have a trailing comma but we want want, then add it
            if (x >= list.SeparatorCount)
            {
                if (closingToken != null)
                {
                    docs.Add(TrailingComma.Print(closingToken.Value, context));
                }

                continue;
            }

            var isTrailingSeparator = x == list.Count - 1;

            if (isTrailingSeparator)
            {
                var trailingSeparatorToken = list.GetSeparator(x);
                // when the trailing separator has trailing comments, we have to print it normally to prevent it from collapsing
                // when the closing token has a directive, we can't assume the comma should be added/removed so just print it normally
                if (
                    trailingSeparatorToken.TrailingTrivia.Any(o => o.IsComment())
                    || closingToken != null
                        && closingToken.Value.LeadingTrivia.Any(o => o.IsDirective)
                )
                {
                    docs.Add(Token.Print(trailingSeparatorToken, context));
                }
                else if (closingToken != null)
                {
                    docs.Add(TrailingComma.Print(closingToken.Value, context));
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
