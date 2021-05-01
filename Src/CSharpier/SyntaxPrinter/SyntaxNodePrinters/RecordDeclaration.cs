using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RecordDeclaration
    {
        public static Doc Print(RecordDeclarationSyntax node)
        {
            return new Printer().PrintBaseTypeDeclarationSyntax(node);
        }
    }
}
