using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Concat(String("from"), String(" "), node.Identifier.Text, String(" "), String("in"), String(" "), this.Print(node.Expression));
        }
    }
}
