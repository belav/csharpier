using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterConstraintClauseSyntax(TypeParameterConstraintClauseSyntax node)
        {
            return Concat(
                String(node.WhereKeyword.Text),
                String(" "),
                this.Print(node.Name),
                String(" : "),
                Join(String(", "), node.Constraints.Select(this.Print))
            );
        }
    }
}
