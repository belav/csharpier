using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPostfixUnaryExpressionSyntax(PostfixUnaryExpressionSyntax node)
        {
            return Concat(this.Print(node.Operand), node.OperatorToken.Text);
        }
    }
}
