using Microsoft.CodeAnalysis;

namespace CSharpier.Core.CSharp;

internal static class SyntaxNodeExtensions
{
    public static T? FindParent<T>(this SyntaxNode? node)
        where T : class
    {
        while (true)
        {
            if (node?.Parent is T t)
            {
                return t;
            }

            if (node is null)
            {
                return null;
            }

            node = node.Parent;
        }
    }

    public static bool HasParent(this SyntaxNode? node, Type theType)
    {
        while (true)
        {
            if (node?.Parent?.GetType() == theType)
            {
                return true;
            }

            if (node is null)
            {
                return false;
            }

            node = node.Parent;
        }
    }
}
