namespace CSharpier.Core.Utilities;

internal static class IEnumerableExtensions
{
    public static bool None<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate
    )
    {
        return !source.Any(predicate);
    }
}