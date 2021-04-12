using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPostfixUnaryExpressionSyntax(
            PostfixUnaryExpressionSyntax node
        ) {
            return Docs.Concat(
                this.Print(node.Operand),
                this.PrintSyntaxToken(node.OperatorToken)
            );
        }
    }
}
