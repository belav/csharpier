using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EventFieldDeclaration
    {
        public static Doc Print(EventFieldDeclarationSyntax node)
        {
            return new Printer().PrintBaseFieldDeclarationSyntax(node);
        }
    }
}
