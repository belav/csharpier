using System.Collections.Generic;
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
                this.PrintModifiers(node.Modifiers),
                this.PrintSyntaxToken(node.Identifier)
            };
            if (node.EqualsValue != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.EqualsValue));
            }
            return Docs.Concat(docs);
        }
    }
}
