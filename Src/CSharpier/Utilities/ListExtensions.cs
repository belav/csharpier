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

    public static bool ManualSkipAny<T>(
        ref this ValueListBuilder<T> collection,
        int count,
        Func<T, bool> predicate
    )
    {
        if (predicate == null || count < 0)
            throw new ArgumentException("Invalid arguments");

        int skipped = 0;
        foreach (var item in collection.AsSpan())
        {
            if (skipped++ < count)
                continue; // Skip the first 'count' items

            if (predicate(item))
                return true; // If any item satisfies the predicate, return true
        }

        return false; // No match found after skipping 'count' items
    }

    public static bool ManualSkipAny<T>(this T[] collection, int count, Func<T, bool> predicate)
    {
        if (collection == null || predicate == null || count < 0)
            throw new ArgumentException("Invalid arguments");

        int skipped = 0;
        foreach (var item in collection)
        {
            if (skipped++ < count)
                continue; // Skip the first 'count' items

            if (predicate(item))
                return true; // If any item satisfies the predicate, return true
        }

        return false; // No match found after skipping 'count' items
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

    public static SyntaxToken[] ToArray(this in SyntaxTokenList source)
    {
        var array = new SyntaxToken[source.Count];
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        for (var i = 0; i < source.Count; i++)
        {
            array[i] = source[i];
        }

        return array;
    }

    public static bool SequenceEqual(this SyntaxToken[] left, in SyntaxTokenList right)
    {
        if (left.Length != right.Count)
        {
            return false;
        }

        for (var i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }

        return true;
    }
}
