using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringTextSyntax(InterpolatedStringTextSyntax node)
        {
            return String(node.TextToken.Text);
        }
    }
}
