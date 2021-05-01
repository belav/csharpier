using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class OperatorDeclaration
    {
        public static Doc Print(OperatorDeclarationSyntax node)
        {
            return new Printer().PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
