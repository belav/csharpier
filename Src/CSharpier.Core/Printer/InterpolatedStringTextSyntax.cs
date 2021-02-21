using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringTextSyntax(InterpolatedStringTextSyntax node)
        {
            return this.PrintSyntaxToken(node.TextToken);
        }
    }
}
