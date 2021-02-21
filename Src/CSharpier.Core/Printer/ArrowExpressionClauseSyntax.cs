using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(ArrowExpressionClauseSyntax node)
        {
            return Group(Indent(Line,
                this.PrintSyntaxToken(node.ArrowToken, " "),
                this.Print(node.Expression)));
        }
    }
}