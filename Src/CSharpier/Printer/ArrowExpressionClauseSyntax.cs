using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(
            ArrowExpressionClauseSyntax node)
        {
            return Docs.Group(
                Docs.Indent(
                    " ",
                    this.PrintSyntaxToken(node.ArrowToken, Docs.Line),
                    this.Print(node.Expression)
                )
            );
        }
    }
}
