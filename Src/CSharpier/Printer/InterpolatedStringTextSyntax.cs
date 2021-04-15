using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolatedStringTextSyntax(
            InterpolatedStringTextSyntax node
        ) {
            return SyntaxTokens.Print(node.TextToken);
        }
    }
}
