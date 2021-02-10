using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPrefixUnaryExpressionSyntax(PrefixUnaryExpressionSyntax node)
        {
            return Concat(node.OperatorToken.Text, this.Print(node.Operand));
        }
    }
}
