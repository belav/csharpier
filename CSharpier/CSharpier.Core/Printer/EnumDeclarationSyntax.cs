using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintEnumDeclarationSyntax(EnumDeclarationSyntax node)
        {
            return this.PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
