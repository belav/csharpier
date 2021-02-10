using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            return this.PrintSyntaxToken(node.Identifier);
        }
    }
}