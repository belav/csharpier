using System.Diagnostics.CodeAnalysis;

namespace CSharpier.Core.Utilities;

internal static class StackExtensions
{
#if NETSTANDARD2_0
    public static bool TryPop<T>(this Stack<T> stack, [NotNullWhen(true)] out T? result)
    {
        if (stack.Count > 0)
        {
            result = stack.Pop()!;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }
#endif

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
