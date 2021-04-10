using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CSharpier
{
    public static class GroupIdGenerator
    {
        [ThreadStatic]
        private static IDictionary<SyntaxNode, string>? groupIds;

        public static string GenerateGroupIdFor(SyntaxNode syntaxNode)
        {
            groupIds ??= new Dictionary<SyntaxNode, string>();

            var value = Guid.NewGuid().ToString();
            groupIds.Add(syntaxNode, value);
            return value;
        }

        public static void RemoveGroupIdFor(SyntaxNode syntaxNode)
        {
            groupIds?.Remove(syntaxNode);
        }

        public static string GroupIdFor(SyntaxNode syntaxNode)
        {
            if (
                groupIds == null
                || !groupIds.TryGetValue(syntaxNode, out var groupId)
            )
            {
                throw new Exception(
                    "There was no groupId for " + syntaxNode.GetType().FullName
                );
            }

            return groupId;
        }
    }
}
