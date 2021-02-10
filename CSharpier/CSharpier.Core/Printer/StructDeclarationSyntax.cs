using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintStructDeclarationSyntax(StructDeclarationSyntax node)
        {
            return this.PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
