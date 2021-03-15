using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(
            ArrowExpressionClauseSyntax node)
        {
            return Group(
                Indent(
                    " ",
                    this.PrintSyntaxToken(node.ArrowToken, Line),
                    this.Print(node.Expression)));
        }
    }
}
