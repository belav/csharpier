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

    // Overload for IEnumerable.ElementAt, prevents allocating Stack<T>.Enumerator
    public static T ElementAt<T>(this Stack<T> collection, int index)
    {
        foreach (var item in collection)
        {
            if (index == 0)
            {
                return item;
            }

            index--;
        }

        throw new ArgumentOutOfRangeException(nameof(index));
    }
}
