using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintLiteralExpressionSyntax(LiteralExpressionSyntax node)
        {
            return this.PrintSyntaxToken(node.Token);
        }
    }
}
