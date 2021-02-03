using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringExpressionSyntax(InterpolatedStringExpressionSyntax node)
        {
            return Concat(
                node.StringStartToken.Text,
                Concat(node.Contents.Select(this.Print).ToArray()),
                node.StringEndToken.Text
            );
        }
    }
}
