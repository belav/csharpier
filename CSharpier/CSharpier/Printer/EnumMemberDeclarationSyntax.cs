using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEnumMemberDeclarationSyntax(EnumMemberDeclarationSyntax node)
        {
            var parts = new Parts();
            this.PrintAttributeLists(node.AttributeLists, parts);
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(node.Identifier.Text);
            if (NotNull(node.EqualsValue)) {
                parts.Push(this.PrintEqualsValueClauseSyntax(node.EqualsValue));
            }
            return Concat(parts);
        }
    }
}
