using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class Modifiers
{
    private class DefaultOrder : IComparer<SyntaxToken>
    {
        public int Compare(SyntaxToken x, SyntaxToken y)
        {
            return GetIndex(x) - GetIndex(y);
        }

        // use the default order from https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0036
        public static int GetIndex(SyntaxToken token)
        {
            return token.RawSyntaxKind() switch
            {
                SyntaxKind.PublicKeyword => 0,
                SyntaxKind.PrivateKeyword => 1,
                SyntaxKind.ProtectedKeyword => 2,
                SyntaxKind.InternalKeyword => 3,
                SyntaxKind.FileKeyword => 4,
                SyntaxKind.StaticKeyword => 5,
                SyntaxKind.ExternKeyword => 6,
                SyntaxKind.NewKeyword => 7,
                SyntaxKind.VirtualKeyword => 8,
#pragma warning disable RSEXPERIMENTAL006
                SyntaxKind.ClosedKeyword => 9,
#pragma warning restore RSEXPERIMENTAL006
                SyntaxKind.AbstractKeyword => 10,
                SyntaxKind.SealedKeyword => 11,
                SyntaxKind.OverrideKeyword => 12,
                SyntaxKind.ReadOnlyKeyword => 13,
                SyntaxKind.UnsafeKeyword => 14,
                SyntaxKind.RequiredKeyword => 15,
                SyntaxKind.VolatileKeyword => 16,
                SyntaxKind.AsyncKeyword => 17,
                _ => int.MaxValue,
            };
        }
    }

    private static readonly DefaultOrder Comparer = new();

    public static Doc Print(SyntaxTokenList modifiers, CSharpPrintingContext context)
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        return Doc.Group(Doc.Join(" ", modifiers, Token.Print, context), " ");
    }

    public static Doc PrintSorted(SyntaxTokenList modifiers, CSharpPrintingContext context)
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        return TryGetSortedModifiers(modifiers, context, out var sortedModifiers)
            ? Doc.Group(Doc.Join(" ", sortedModifiers, Token.Print, context), " ")
            : Doc.Group(Doc.Join(" ", modifiers, Token.Print, context), " ");
    }

    // returns false when the modifiers should be printed as they are, so that the common case of
    // an already sorted list costs neither an array nor a sort
    private static bool TryGetSortedModifiers(
        in SyntaxTokenList modifiers,
        CSharpPrintingContext context,
        out SyntaxToken[] sortedModifiers
    )
    {
        sortedModifiers = [];

        if (modifiers.Count <= 1 || !CanReorderModifiers(modifiers) || IsSorted(modifiers))
        {
            return false;
        }

        sortedModifiers = modifiers.ToArray();
        var leadingToken = sortedModifiers[0];
        Array.Sort(sortedModifiers, Comparer);

        if (sortedModifiers.SequenceEqual(modifiers))
        {
            return false;
        }

        context.State.ReorderedModifiers = true;

        var leadingTrivia = leadingToken.LeadingTrivia;
        var leadingTokenIndex = Array.IndexOf(sortedModifiers, leadingToken);
        sortedModifiers[leadingTokenIndex] = sortedModifiers[leadingTokenIndex]
            .WithLeadingTrivia(new SyntaxTriviaList());
        sortedModifiers[0] = sortedModifiers[0].WithLeadingTrivia(leadingTrivia);

        return true;
    }

    // reordering modifiers inside of #ifs can lead to code that doesn't compile
    private static bool CanReorderModifiers(in SyntaxTokenList modifiers)
    {
        if (modifiers[0].LeadingTrivia.AnyDirective())
        {
            return false;
        }

        for (var index = 1; index < modifiers.Count; index++)
        {
            if (modifiers[index].LeadingTrivia.AnyCommentOrDirective())
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSorted(in SyntaxTokenList modifiers)
    {
        var previous = DefaultOrder.GetIndex(modifiers[0]);
        for (var index = 1; index < modifiers.Count; index++)
        {
            var current = DefaultOrder.GetIndex(modifiers[index]);
            if (current < previous)
            {
                return false;
            }

            previous = current;
        }

        return true;
    }
}
