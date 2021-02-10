using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintRefExpressionSyntax(RefExpressionSyntax node)
        {
            return Concat(node.RefKeyword.Text, " ", this.Print(node.Expression));
        }
    }
}
