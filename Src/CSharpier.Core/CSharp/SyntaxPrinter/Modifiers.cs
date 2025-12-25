using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class Modifiers
{
    private class DefaultOrder : IComparer<SyntaxToken>
    {
        // use the default order from https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0036
        private static readonly string[] DefaultOrdered =
        [
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
        ];

        public int Compare(SyntaxToken x, SyntaxToken y)
        {
            return GetIndex(x.Text) - GetIndex(y.Text);
        }

        private static int GetIndex(string? value)
        {
            var result = Array.IndexOf(DefaultOrdered, value);
            return result == -1 ? int.MaxValue : result;
        }
    }

    private static readonly DefaultOrder Comparer = new();

    public static Doc Print(SyntaxTokenList modifiers, PrintingContext context)
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        return Doc.Group(Doc.Join(" ", modifiers.Select(o => Token.Print(o, context))), " ");
    }

    public static Doc PrintSorted(SyntaxTokenList modifiers, PrintingContext context)
    {
        return PrintWithSortedModifiers(
            modifiers,
            context,
            sortedModifiers =>
                Doc.Group(Doc.Join(" ", sortedModifiers.Select(o => Token.Print(o, context))), " ")
        );
    }

    public static Doc PrintSorterWithoutLeadingTrivia(
        SyntaxTokenList modifiers,
        PrintingContext context
    )
    {
        return PrintWithSortedModifiers(
            modifiers,
            context,
            sortedModifiers =>
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(sortedModifiers[0], context),
                    " ",
                    sortedModifiers.Count > 1
                        ? Doc.Concat(
                            sortedModifiers
                                .Skip(1)
                                .Select(o => Token.PrintWithSuffix(o, " ", context))
                                .ToArray()
                        )
                        : Doc.Null
                )
        );
    }

    private static Doc PrintWithSortedModifiers(
        in SyntaxTokenList modifiers,
        PrintingContext context,
        Func<IReadOnlyList<SyntaxToken>, Doc> print
    )
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        // reordering modifiers inside of #ifs can lead to code that doesn't compile
        var willReorderModifiers =
            modifiers.Count > 1
            && !modifiers.Skip(1).Any(o => o.LeadingTrivia.Any(p => p.IsDirective || p.IsComment()))
            && !modifiers[0].LeadingTrivia.Any(p => p.IsDirective);

        var sortedModifiers = modifiers.ToArray();
        var leadingToken = sortedModifiers.FirstOrDefault();
        if (willReorderModifiers)
        {
            Array.Sort(sortedModifiers, Comparer);
        }

        if (willReorderModifiers && !sortedModifiers.SequenceEqual(modifiers))
        {
            context.State.ReorderedModifiers = true;

            var leadingTrivia = leadingToken.LeadingTrivia;
            var leadingTokenIndex = Array.FindIndex(
                sortedModifiers,
                token => token == leadingToken
            );
            sortedModifiers[leadingTokenIndex] = sortedModifiers[leadingTokenIndex]
                .WithLeadingTrivia(new SyntaxTriviaList());
            sortedModifiers[0] = sortedModifiers[0].WithLeadingTrivia(leadingTrivia);
        }

        return print(sortedModifiers);
    }
}
