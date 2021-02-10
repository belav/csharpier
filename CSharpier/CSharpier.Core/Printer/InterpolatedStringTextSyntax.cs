using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringTextSyntax(InterpolatedStringTextSyntax node)
        {
            return node.TextToken.Text;
        }
    }
}
