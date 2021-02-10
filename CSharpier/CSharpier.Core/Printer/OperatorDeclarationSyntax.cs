using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintOperatorDeclarationSyntax(OperatorDeclarationSyntax node)
        {
            return this.PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
