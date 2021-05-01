using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class InterfaceDeclaration
    {
        public static Doc Print(InterfaceDeclarationSyntax node)
        {
            return new Printer().PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
