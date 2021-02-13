using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintUnsafeStatementSyntax(UnsafeStatementSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.UnsafeKeyword), this.Print(node.Block));
        }
    }
}
