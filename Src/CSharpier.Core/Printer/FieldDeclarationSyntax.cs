using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFieldDeclarationSyntax(FieldDeclarationSyntax node)
        {
            return this.PrintBaseFieldDeclarationSyntax(node);
        }
    }
}
