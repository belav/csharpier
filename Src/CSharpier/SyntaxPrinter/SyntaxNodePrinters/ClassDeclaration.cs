using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ClassDeclaration
    {
        public static Doc Print(ClassDeclarationSyntax node)
        {
            return new Printer().PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
