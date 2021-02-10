using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTypeParameterConstraintClauseSyntax(TypeParameterConstraintClauseSyntax node)
        {
            return Concat(
                node.WhereKeyword.Text,
                " ",
                this.Print(node.Name),
                " : ",
                Join(", ", node.Constraints.Select(this.Print))
            );
        }
    }
}
