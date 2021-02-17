using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 0 how do I go about preventing breaks here? should I make a group that removes all hardlines, lines, etc within it??
        private Doc PrintInterpolatedStringExpressionSyntax(InterpolatedStringExpressionSyntax node)
        {
            return ForceFlat(
                this.PrintSyntaxToken(node.StringStartToken),
                Concat(node.Contents.Select(this.Print).ToArray()),
                this.PrintSyntaxToken(node.StringEndToken)
            );
        }
    }
}
