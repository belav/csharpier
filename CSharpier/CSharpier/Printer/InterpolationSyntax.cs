using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolationSyntax(InterpolationSyntax node)
        {
            var parts = new Parts(String("{"), this.Print(node.Expression));
            if (node.AlignmentClause != null) {
                parts.Push(String(", "), this.Print(node.AlignmentClause.Value));
            }
            if (node.FormatClause != null) {
                parts.Push(String(":"), node.FormatClause.FormatStringToken.Text);
            }
            parts.Add(String("}"));
            return Concat(parts);
        }
    }
}
