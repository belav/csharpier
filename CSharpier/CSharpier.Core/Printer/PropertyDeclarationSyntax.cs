using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPropertyDeclarationSyntax(PropertyDeclarationSyntax node)
        {
            return this.PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
