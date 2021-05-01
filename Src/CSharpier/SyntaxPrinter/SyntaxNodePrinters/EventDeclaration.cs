using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EventDeclaration
    {
        public static Doc Print(EventDeclarationSyntax node)
        {
            return new Printer().PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
