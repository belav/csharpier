using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefValueExpressionSyntax(
            RefValueExpressionSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Token.Print(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.Comma, afterTokenIfNoTrailing: " "),
                this.Print(node.Type),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
