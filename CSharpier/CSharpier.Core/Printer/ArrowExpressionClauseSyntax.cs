using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(ArrowExpressionClauseSyntax node)
        {
            return Concat(" => ", this.Print(node.Expression));
        }
    }
}
