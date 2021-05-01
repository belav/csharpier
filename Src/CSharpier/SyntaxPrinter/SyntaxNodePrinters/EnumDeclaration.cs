using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EnumDeclaration
    {
        public static Doc Print(EnumDeclarationSyntax node)
        {
            return new Printer().PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
