using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentSyntax(ArgumentSyntax node)
        {
            var parts = new Parts();
            if (NotNull(node.NameColon)) {
                parts.Push(this.PrintNameColonSyntax(node.NameColon));
            }
            if (NotNull(node.RefKindKeyword)) {
                parts.Push(String(node.RefKindKeyword.Text), String(" "));
            }
            parts.Push(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
