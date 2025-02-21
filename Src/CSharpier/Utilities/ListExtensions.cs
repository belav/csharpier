namespace CSharpier.Utilities;

internal static class ListExtensions
{
    public static void Add(this List<Doc> list, params Doc[] values)
    {
        if (values.Length == 1 && values[0] == Doc.Null)
        {
            return;
        }
        list.AddRange(values);
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
}
