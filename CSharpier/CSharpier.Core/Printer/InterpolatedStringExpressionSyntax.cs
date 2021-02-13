using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringExpressionSyntax(InterpolatedStringExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.StringStartToken),
                Concat(node.Contents.Select(this.Print).ToArray()),
                this.PrintSyntaxToken(node.StringEndToken)
            );
        }
    }
}
