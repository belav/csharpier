using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnsafeStatementSyntax(UnsafeStatementSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.UnsafeKeyword),
                this.Print(node.Block)
            );
        }
    }
}
