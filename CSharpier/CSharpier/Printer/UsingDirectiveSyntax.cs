using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            var parts = new Parts();
            parts.Add(String("using "));
            if (NotNullToken(node.StaticKeyword)) {
                parts.Add(String("static "));
            }
            if (node.Alias != null) {
                parts.Add(this.PrintNameEqualsSyntax(node.Alias));
            }
            parts.Push(Group(this.Print(node.Name)), String(";"));
            return Concat(parts);
        }
    }
}
