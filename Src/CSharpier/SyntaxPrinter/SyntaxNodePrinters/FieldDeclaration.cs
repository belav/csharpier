using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class FieldDeclaration
    {
        public static Doc Print(FieldDeclarationSyntax node)
        {
            return new Printer().PrintBaseFieldDeclarationSyntax(node);
        }
    }
}
