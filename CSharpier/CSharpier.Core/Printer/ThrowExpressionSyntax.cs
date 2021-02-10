using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintThrowExpressionSyntax(ThrowExpressionSyntax node)
        {
            return Concat(node.ThrowKeyword.Text, " ", this.Print(node.Expression));
        }
    }
}
