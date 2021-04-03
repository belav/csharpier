using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.WhileKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                Group(Indent(SoftLine, this.Print(node.Condition))),
                // TODO 1 maybe the close paraen should be on a new line, that would aid in difs for code reviews
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Statement)
            );
        }
    }
}
