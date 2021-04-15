using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConversionOperatorDeclarationSyntax(
            ConversionOperatorDeclarationSyntax node
        ) {
            return this.PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
