using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnsafeStatementSyntax(UnsafeStatementSyntax node)
        {
            return Docs.Concat(
                SyntaxTokens.Print(node.UnsafeKeyword),
                this.Print(node.Block)
            );
        }
    }
}
