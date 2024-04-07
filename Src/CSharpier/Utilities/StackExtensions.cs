namespace CSharpier.Utilities;

using System.Diagnostics.CodeAnalysis;

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
}
