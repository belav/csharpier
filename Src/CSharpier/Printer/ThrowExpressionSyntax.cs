using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThrowExpressionSyntax(ThrowExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.ThrowKeyword, " "),
                this.Print(node.Expression));
        }
    }
}
