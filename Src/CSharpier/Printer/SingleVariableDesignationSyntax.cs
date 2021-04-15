using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSingleVariableDesignationSyntax(
            SingleVariableDesignationSyntax node
        ) {
            return SyntaxTokens.Print(node.Identifier);
        }
    }
}
