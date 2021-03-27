using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAwaitExpressionSyntax(AwaitExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.AwaitKeyword, " "),
                this.Print(node.Expression)
            );
        }
    }
}
