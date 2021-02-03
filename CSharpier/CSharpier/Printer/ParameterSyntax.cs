using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterSyntax(ParameterSyntax node)
        {
            var parts = new Parts();
            if (node.AttributeLists != null) {
                this.PrintAttributeLists(node.AttributeLists, parts);
            }
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (NotNull(node.Type)) {
                parts.Push(this.Print(node.Type), String(" "));
            }
            parts.Push(node.Identifier.Text);
            if (NotNull(node.Default)) {
                parts.Push(this.PrintEqualsValueClauseSyntax(node.Default));
            }
            return Concat(parts);
        }
    }
}
