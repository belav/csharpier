using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPrefixUnaryExpressionSyntax(PrefixUnaryExpressionSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.OperatorToken), this.Print(node.Operand));
        }
    }
}
