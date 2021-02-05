using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterSyntax(ParameterSyntax node)
        {
            var parts = new Parts();
            this.PrintAttributeLists(node, node.AttributeLists, parts);

            parts.Add(this.PrintModifiers(node.Modifiers));
            if (node.Type != null)
            {
                parts.Push(this.Print(node.Type), String(" "));
            }

            parts.Add(node.Identifier.Text);
            if (node.Default != null)
            {
                parts.Add(this.PrintEqualsValueClauseSyntax(node.Default));
            }

            return Concat(parts);
        }
    }
}