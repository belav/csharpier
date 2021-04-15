using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDiscardDesignationSyntax(
            DiscardDesignationSyntax node
        ) {
            return SyntaxTokens.Print(node.UnderscoreToken);
        }
    }
}
