using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedExpressionSyntax(
            ParenthesizedExpressionSyntax node
        ) {
            return Concat(
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
