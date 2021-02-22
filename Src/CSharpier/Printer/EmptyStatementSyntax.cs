using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEmptyStatementSyntax(EmptyStatementSyntax node)
        {
            return this.PrintSyntaxToken(node.SemicolonToken);
        }
    }
}
