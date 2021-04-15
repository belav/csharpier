using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLiteralExpressionSyntax(LiteralExpressionSyntax node)
        {
            return SyntaxTokens.Print(node.Token);
        }
    }
}
