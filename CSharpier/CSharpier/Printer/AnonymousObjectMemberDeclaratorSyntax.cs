using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectMemberDeclaratorSyntax(AnonymousObjectMemberDeclaratorSyntax node)
        {
            var parts = new Parts();
            if (node.NameEquals != null) {
                parts.Push(this.PrintIdentifierNameSyntax(node.NameEquals.Name));
                parts.Push(String(" = "));
            }
            parts.Push(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
