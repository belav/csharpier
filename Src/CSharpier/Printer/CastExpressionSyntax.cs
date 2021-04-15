using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCastExpressionSyntax(CastExpressionSyntax node)
        {
            return Docs.Concat(
                SyntaxTokens.Print(node.OpenParenToken),
                this.Print(node.Type),
                SyntaxTokens.Print(node.CloseParenToken),
                this.Print(node.Expression)
            );
        }
    }
}
