using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEventDeclarationSyntax(EventDeclarationSyntax node)
        {
            return this.PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
