using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class MethodDeclaration
    {
        public static Doc Print(MethodDeclarationSyntax node)
        {
            return new Printer().PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
