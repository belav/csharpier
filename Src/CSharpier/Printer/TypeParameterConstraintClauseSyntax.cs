using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterConstraintClauseSyntax(
            TypeParameterConstraintClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.WhereKeyword, " "),
                this.Print(node.Name),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.ColonToken, " "),
                this.PrintSeparatedSyntaxList(
                    node.Constraints,
                    this.Print,
                    " "));
        }
    }
}
