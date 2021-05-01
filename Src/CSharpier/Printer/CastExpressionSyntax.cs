using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCastExpressionSyntax(CastExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                this.Print(node.Type),
                Token.Print(node.CloseParenToken),
                this.Print(node.Expression)
            );
        }
    }
}
