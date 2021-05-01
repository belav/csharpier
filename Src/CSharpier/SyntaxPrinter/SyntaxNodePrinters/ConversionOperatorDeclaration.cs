using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConversionOperatorDeclaration
    {
        public static Doc Print(ConversionOperatorDeclarationSyntax node)
        {
            return new Printer().PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
