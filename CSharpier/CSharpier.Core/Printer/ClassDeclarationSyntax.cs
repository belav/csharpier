using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintClassDeclarationSyntax(ClassDeclarationSyntax node)
        {
            return this.PrintBaseTypeDeclarationSyntax(node);
        }
    }
}