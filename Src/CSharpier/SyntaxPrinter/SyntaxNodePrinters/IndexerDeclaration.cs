using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IndexerDeclaration
    {
        public static Doc Print(IndexerDeclarationSyntax node)
        {
            return new Printer().PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
