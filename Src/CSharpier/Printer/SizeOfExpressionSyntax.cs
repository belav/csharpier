using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSizeOfExpressionSyntax(SizeOfExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.Keyword),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Type),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
