using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCastExpressionSyntax(CastExpressionSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Type),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Expression));
        }
    }
}