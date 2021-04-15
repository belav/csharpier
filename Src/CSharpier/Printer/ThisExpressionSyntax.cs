using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThisExpressionSyntax(ThisExpressionSyntax node)
        {
            return SyntaxTokens.Print(node.Token);
        }
    }
}
