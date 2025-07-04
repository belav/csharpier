using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

    // Overload for IEnumerable.ElementAt, prevents allocating Stack<T>.Enumerator
    public static T ElementAt<T>(this Stack<T> collection, int index)
    {
        throw new ArgumentOutOfRangeException(nameof(index));

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
#else

    // Overload for IEnumerable.ElementAt, prevents allocating Stack<T>.Enumerator
    public static T ElementAt<T>(this Stack<T> collection, int index)
    {
        ref var arrayRef = ref StackAccessor<T>.GetArray(collection);
        return arrayRef[collection.Count - 1 - index];
    }
}

internal static class StackAccessor<T>
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_array")]
    public static extern ref T[] GetArray(Stack<T> stack);
}
#endif
