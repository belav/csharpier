using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAwaitExpressionSyntax(AwaitExpressionSyntax node)
        {
            return Concat("await", " ", this.Print(node.Expression));
        }
    }
}
