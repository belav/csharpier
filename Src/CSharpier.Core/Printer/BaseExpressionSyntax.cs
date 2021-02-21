using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBaseExpressionSyntax(BaseExpressionSyntax node)
        {
            return this.PrintSyntaxToken(node.Token);
        }
    }
}
