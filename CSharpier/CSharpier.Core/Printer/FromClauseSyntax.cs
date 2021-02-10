using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Concat("from", " ", node.Identifier.Text, " ", "in", " ", this.Print(node.Expression));
        }
    }
}
