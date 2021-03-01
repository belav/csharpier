using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.WhileKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                Group(Indent(SoftLine,
                    this.Print(node.Condition))),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Statement));
        }
    }
}