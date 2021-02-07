using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAwaitExpressionSyntax(AwaitExpressionSyntax node)
        {
            return Concat("await", " ", this.Print(node.Expression));
        }
    }
}
