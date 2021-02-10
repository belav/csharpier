using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintEnumMemberDeclarationSyntax(EnumMemberDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(node.Identifier.Text);
            if (node.EqualsValue != null) {
                parts.Add(this.PrintEqualsValueClauseSyntax(node.EqualsValue));
            }
            return Concat(parts);
        }
    }
}
