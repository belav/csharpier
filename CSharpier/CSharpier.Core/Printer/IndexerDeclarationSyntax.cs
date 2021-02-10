using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintIndexerDeclarationSyntax(IndexerDeclarationSyntax node)
        {
            return this.PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
