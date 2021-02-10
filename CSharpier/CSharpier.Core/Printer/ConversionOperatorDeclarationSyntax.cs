using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConversionOperatorDeclarationSyntax(ConversionOperatorDeclarationSyntax node)
        {
            return this.PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
