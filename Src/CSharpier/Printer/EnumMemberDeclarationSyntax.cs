using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEnumMemberDeclarationSyntax(
            EnumMemberDeclarationSyntax node
        ) {
            var docs = new List<Doc>
            {
                this.PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.Identifier)
            };
            if (node.EqualsValue != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.EqualsValue));
            }
            return Doc.Concat(docs);
        }
    }
}
