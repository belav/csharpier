using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEqualsValueClauseSyntax(EqualsValueClauseSyntax node)
        {
            return Concat(String(" = "), this.Print(node.Value));
        }
    }
}
