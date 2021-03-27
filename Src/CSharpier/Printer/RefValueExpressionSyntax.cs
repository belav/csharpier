using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefValueExpressionSyntax(RefValueExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.Keyword),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.Comma, " "),
                this.Print(node.Type),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
