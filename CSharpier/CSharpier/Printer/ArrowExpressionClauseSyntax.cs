using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrowExpressionClauseSyntax(ArrowExpressionClauseSyntax node)
        {
            return Concat(String(" => "), this.Print(node.Expression));
        }
    }
}
