using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPrefixUnaryExpressionSyntax(PrefixUnaryExpressionSyntax node)
        {
            return Concat(String(node.OperatorToken.Text), this.Print(node.Operand));
        }
    }
}
