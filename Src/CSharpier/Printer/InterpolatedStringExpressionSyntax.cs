using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringExpressionSyntax(
            InterpolatedStringExpressionSyntax node
        ) {
            return Docs.ForceFlat(
                SyntaxTokens.Print(node.StringStartToken),
                Docs.Concat(node.Contents.Select(this.Print).ToArray()),
                SyntaxTokens.Print(node.StringEndToken)
            );
        }
    }
}
