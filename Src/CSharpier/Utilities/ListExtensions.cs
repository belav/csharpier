namespace CSharpier.Utilities;

internal static class ListExtensions
{
    public static void Add(this List<Doc> list, params ReadOnlySpan<Doc> values)
    {
        if (values.Length == 1 && values[0] == Doc.Null)
        {
            return;
        }

#if NETSTANDARD2_0
        foreach (var items in values)
        {
            list.Add(items);
        }
#else
        list.AddRange(values);
#endif
    }

    public static void AddIfNotNull(this List<Doc> value, Doc doc)
    {
        if (doc != Doc.Null)
        {
            value.Add(doc);
        }
    }

    // Overload for Any to prevent unnecessary allocations of EnumeratorImpl
    public static bool Any(this in SyntaxTriviaList triviaList, Func<SyntaxTrivia, bool> predicate)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var trivia in triviaList)
        {
            if (predicate(trivia))
            {
                return true;
            }
        }

        return false;
    }

    // Overload for Any to prevent unnecessary allocations of EnumeratorImpl
    public static bool Any(this in SyntaxTokenList tokenList, Func<SyntaxToken, bool> predicate)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var token in tokenList)
        {
            if (predicate(token))
            {
                return true;
            }
        }

        return false;
    }

    public static SyntaxTrivia FirstOrDefault(
        this in SyntaxTriviaList source,
        Func<SyntaxTrivia, bool> predicate
    )
    {
        var first = new SyntaxTrivia();
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var trivia in source)
        {
            if (predicate(trivia))
            {
                first = trivia;
                break;
            }
        }

        return first;
    }
}
