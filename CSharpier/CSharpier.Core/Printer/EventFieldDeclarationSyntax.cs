using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintEventFieldDeclarationSyntax(EventFieldDeclarationSyntax node)
        {
            return this.PrintBaseFieldDeclarationSyntax(node);
        }
    }
}
