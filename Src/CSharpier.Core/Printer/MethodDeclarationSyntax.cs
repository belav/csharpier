using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintMethodDeclarationSyntax(MethodDeclarationSyntax node)
        {
            return this.PrintBaseMethodDeclarationSyntax(node);
        }
    }
}