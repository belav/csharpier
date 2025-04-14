using Microsoft.CodeAnalysis;

namespace CSharpier.Core;

internal static class SyntaxNodeExtensions
{
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
