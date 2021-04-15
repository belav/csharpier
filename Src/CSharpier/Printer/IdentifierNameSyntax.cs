using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            return SyntaxTokens.Print(node.Identifier);
        }
    }
}
