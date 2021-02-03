using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            var parts = new Parts();
            parts.Push(String("using "));
            if (NotNull(node.StaticKeyword)) {
                parts.Push(String("static "));
            }
            if (NotNull(node.Alias)) {
                parts.Push(this.PrintNameEqualsSyntax(node.Alias));
            }
            parts.Push(Group(this.Print(node.Name)), String(";"));
            return Concat(parts);
        }
    }
}
