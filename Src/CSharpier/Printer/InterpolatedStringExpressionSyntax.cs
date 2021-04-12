using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringExpressionSyntax(
            InterpolatedStringExpressionSyntax node
        ) {
            return Docs.ForceFlat(
                this.PrintSyntaxToken(node.StringStartToken),
                Docs.Concat(node.Contents.Select(this.Print).ToArray()),
                this.PrintSyntaxToken(node.StringEndToken)
            );
        }
    }
}
