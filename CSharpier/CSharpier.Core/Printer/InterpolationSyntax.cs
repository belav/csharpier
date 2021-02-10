using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInterpolationSyntax(InterpolationSyntax node)
        {
            var parts = new Parts("{", this.Print(node.Expression));
            if (node.AlignmentClause != null) {
                parts.Push(", ", this.Print(node.AlignmentClause.Value));
            }
            if (node.FormatClause != null) {
                parts.Push(":", node.FormatClause.FormatStringToken.Text);
            }
            parts.Add("}");
            return Concat(parts);
        }
    }
}
