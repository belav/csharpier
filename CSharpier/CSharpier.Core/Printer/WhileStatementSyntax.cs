using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.WhileKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Condition),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Statement)
            );
        }
    }
}
