using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintContinueStatementSyntax(ContinueStatementSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(node.ContinueKeyword),
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
