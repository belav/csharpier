#if NETSTANDARD2_0
namespace CSharpier.Utilities;

internal static class DictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue
    )
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }
}
#endif
