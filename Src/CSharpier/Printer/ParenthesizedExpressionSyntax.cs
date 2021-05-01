using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedExpressionSyntax(
            ParenthesizedExpressionSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                this.Print(node.Expression),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
