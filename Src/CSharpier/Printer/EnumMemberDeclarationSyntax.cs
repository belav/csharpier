using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEnumMemberDeclarationSyntax(
            EnumMemberDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintSyntaxToken(node.Identifier));
            if (node.EqualsValue != null)
            {
                parts.Push(this.PrintEqualsValueClauseSyntax(node.EqualsValue));
            }
            return Concat(parts);
        }
    }
}
