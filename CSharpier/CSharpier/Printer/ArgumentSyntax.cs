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
            if (NotNullToken(node.RefKindKeyword)) {
                parts.Push(String(node.RefKindKeyword.Text), String(" "));
            }
            parts.Add(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
