using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintEventDeclarationSyntax(EventDeclarationSyntax node)
        {
            return this.PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
