using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPostfixUnaryExpressionSyntax(
            PostfixUnaryExpressionSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Operand),
                Token.Print(node.OperatorToken)
            );
        }
    }
}
