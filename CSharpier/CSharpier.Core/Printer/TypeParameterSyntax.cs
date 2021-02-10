using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTypeParameterSyntax(TypeParameterSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            if (node.VarianceKeyword.RawKind != 0) {
                parts.Push(node.VarianceKeyword.Text, " ");
            }
            parts.Add(node.Identifier.Text);
            return Concat(parts);
        }
    }
}
