using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(
            ArrowExpressionClauseSyntax node
        ) {
            return Doc.Group(
                Doc.Indent(
                    " ",
                    this.PrintSyntaxToken(node.ArrowToken, Doc.Line),
                    this.Print(node.Expression)
                )
            );
        }
    }
}
