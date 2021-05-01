using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class PropertyDeclaration
    {
        public static Doc Print(PropertyDeclarationSyntax node)
        {
            return new Printer().PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
