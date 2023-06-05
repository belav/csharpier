namespace CSharpier.SyntaxPrinter;

internal static class Modifiers
{
    private class DefaultOrder : IComparer<string>
    {
        // use the default order from https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0036
        private static readonly string[] DefaultOrdered =
        {
            "public",
            "private",
            "protected",
            "internal",
            "file",
            "static",
            "extern",
            "new",
            "virtual",
            "abstract",
            "sealed",
            "override",
            "readonly",
            "unsafe",
            "required",
            "volatile",
            "async",
        };

        public int Compare(string? x, string? y)
        {
            int GetIndex(string? value)
            {
                var result = Array.IndexOf(DefaultOrdered, value);
                return result == -1 ? int.MaxValue : result;
            }

            return GetIndex(x) - GetIndex(y);
        }
    }

    private static readonly DefaultOrder Comparer = new();

    public static Doc Print(SyntaxTokenList modifiers, FormattingContext context)
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        return Doc.Group(
            Doc.Join(
                " ",
                modifiers.OrderBy(o => o.Text, Comparer).Select(o => Token.Print(o, context))
            ),
            " "
        );
    }

    public static Doc PrintWithoutLeadingTrivia(
        SyntaxTokenList modifiers,
        FormattingContext context
    )
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        // reordering modifiers inside of #ifs can lead to code that doesn't compile
        var sortedModifiers = modifiers.Any(o => o.LeadingTrivia.Any(p => p.IsDirective))
            ? modifiers.AsEnumerable()
            : modifiers.OrderBy(o => o.Text, Comparer);

        return Doc.Group(
            Token.PrintWithoutLeadingTrivia(sortedModifiers.First(), context),
            " ",
            sortedModifiers.Count() > 1
                ? Doc.Concat(
                    sortedModifiers
                        .Skip(1)
                        .Select(o => Token.PrintWithSuffix(o, " ", context))
                        .ToArray()
                )
                : Doc.Null
        );
    }
}
