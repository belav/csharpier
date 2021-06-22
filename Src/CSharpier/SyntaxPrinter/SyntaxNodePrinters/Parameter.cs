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

            var paramDocs = new List<Doc>();
            if (node.Type != null)
            {
                paramDocs.Add(Node.Print(node.Type), " ");
            }

            paramDocs.Add(Token.Print(node.Identifier));
            if (node.Default != null)
            {
                paramDocs.Add(EqualsValueClause.Print(node.Default));
            }

            if (hasAttribute)
            {
                docs.Add(Doc.Concat(paramDocs));
                return Doc.GroupWithId(groupId, docs.ToArray());
            }
            else
            {
                docs.AddRange(paramDocs);
                return Doc.Concat(docs);
            }
        }
    }
}
