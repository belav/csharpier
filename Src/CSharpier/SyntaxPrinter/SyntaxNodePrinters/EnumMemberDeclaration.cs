using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class EnumMemberDeclaration
    {
        public static Doc Print(EnumMemberDeclarationSyntax node)
        {
            var docs = new List<Doc>
            {
                AttributeLists.Print(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.Identifier)
            };
            if (node.EqualsValue != null)
            {
                docs.Add(EqualsValueClause.Print(node.EqualsValue));
            }
            return Doc.Concat(docs);
        }
    }
}
