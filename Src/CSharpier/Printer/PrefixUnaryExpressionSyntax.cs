using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPrefixUnaryExpressionSyntax(
            PrefixUnaryExpressionSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.OperatorToken),
                this.Print(node.Operand)
            );
        }
    }
}
