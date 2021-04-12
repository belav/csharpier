using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterConstraintClauseSyntax(
            TypeParameterConstraintClauseSyntax node
        ) {
            return Docs.Group(
                this.PrintSyntaxToken(
                    node.WhereKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Name),
                Docs.SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(
                    node.ColonToken,
                    afterTokenIfNoTrailing: " "
                ),
                Docs.Indent(
                    this.PrintSeparatedSyntaxList(
                        node.Constraints,
                        this.Print,
                        Docs.Line
                    )
                )
            );
        }
    }
}
