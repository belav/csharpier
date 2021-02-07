using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentSyntax(ArgumentSyntax node)
        {
            var parts = new Parts();
            if (node.NameColon != null) {
                parts.Add(this.PrintNameColonSyntax(node.NameColon));
            }
            if (node.RefKindKeyword.RawKind != 0) {
                parts.Push(node.RefKindKeyword.Text, " ");
            }
            parts.Add(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
