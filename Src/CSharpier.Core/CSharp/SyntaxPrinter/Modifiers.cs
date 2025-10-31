using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class Modifiers
{
    private class DefaultOrder : IComparer<SyntaxToken>
    {
        public int Compare(SyntaxToken x, SyntaxToken y)
        {
            return GetIndex(x.Text) - GetIndex(y.Text);
        }

        private static int GetIndex(string? value)
        {
            // use the default order from https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0036
            return value switch
            {
                "public" => 0,
                "private" => 1,
                "protected" => 2,
                "internal" => 3,
                "file" => 4,
                "static" => 5,
                "extern" => 6,
                "new" => 7,
                "virtual" => 8,
                "abstract" => 9,
                "sealed" => 10,
                "override" => 11,
                "readonly" => 12,
                "unsafe" => 13,
                "required" => 14,
                "volatile" => 15,
                "async" => 16,
                _ => int.MaxValue,
            };
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
            static (sortedModifiers, context) =>
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
            static (sortedModifiers, context) =>
            {
                var docs = new ValueListBuilder<Doc>(
                    [null, null, null, null, null, null, null, null]
                );
                docs.Append(Token.PrintWithoutLeadingTrivia(sortedModifiers[0], context));
                docs.Append(" ");

                for (int i = 1; i < sortedModifiers.Count; i++)
                {
                    docs.Append(Token.PrintWithSuffix(sortedModifiers[i], " ", context));
                }

                var returnDoc = Doc.Group(ref docs);
                docs.Dispose();
                return returnDoc;
            }
        );
    }

    private static Doc PrintWithSortedModifiers(
        in SyntaxTokenList modifiers,
        PrintingContext context,
        Func<IReadOnlyList<SyntaxToken>, PrintingContext, Doc> print
    )
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        // reordering modifiers inside of #ifs can lead to code that doesn't compile
        var willReorderModifiers =
            modifiers.Count > 1
            && !modifiers.Any(o => o.LeadingTrivia.Any(p => p.IsDirective || p.IsComment()));

        var sortedModifiers = modifiers.ToArray();
        if (willReorderModifiers)
        {
            Array.Sort(sortedModifiers, Comparer);
        }

        if (willReorderModifiers && !sortedModifiers.SequenceEqual(modifiers))
        {
            context.State.ReorderedModifiers = true;
        }

        return print(sortedModifiers, context);
    }
}
