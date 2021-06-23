using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class Parameter
    {
        public static Doc Print(ParameterSyntax node)
        {
            var hasAttribute = node.AttributeLists.Any();
            var groupId = hasAttribute ? Guid.NewGuid().ToString() : string.Empty;
            var docs = new List<Doc>
            {
                AttributeLists.Print(node, node.AttributeLists),
                hasAttribute ? Doc.IndentIfBreak(Doc.Line, groupId) : Doc.Null,
                Modifiers.Print(node.Modifiers)
            };

            if (node.Type != null)
            {
                docs.Add(Node.Print(node.Type), " ");
            }

            docs.Add(Token.Print(node.Identifier));
            if (node.Default != null)
            {
                docs.Add(EqualsValueClause.Print(node.Default));
            }

            return hasAttribute ? Doc.GroupWithId(groupId, docs) : Doc.Concat(docs);
        }
    }
}
