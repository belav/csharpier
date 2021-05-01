using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EnumMemberDeclaration
    {
        public static Doc Print(EnumMemberDeclarationSyntax node)
        {
            var docs = new List<Doc>
            {
                new Printer().PrintAttributeLists(node, node.AttributeLists),
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
