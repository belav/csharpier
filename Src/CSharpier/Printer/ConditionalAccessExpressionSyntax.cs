using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalAccessExpressionSyntax(
            ConditionalAccessExpressionSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Expression),
                Token.Print(node.OperatorToken),
                this.Print(node.WhenNotNull)
            );
        }
    }
}
