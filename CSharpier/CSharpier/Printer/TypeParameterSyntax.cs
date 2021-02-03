using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterSyntax(TypeParameterSyntax node)
        {
            var parts = new Parts();
            this.PrintAttributeLists(node.AttributeLists, parts);
            if (NotNull(node.VarianceKeyword)) {
                parts.Push(node.VarianceKeyword.Text, String(" "));
            }
            parts.Push(node.Identifier.Text);
            return Concat(parts);
        }
    }
}
