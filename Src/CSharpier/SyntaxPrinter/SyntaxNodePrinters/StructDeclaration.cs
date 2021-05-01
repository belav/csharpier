using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class StructDeclaration
    {
        public static Doc Print(StructDeclarationSyntax node)
        {
            return new Printer().PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
