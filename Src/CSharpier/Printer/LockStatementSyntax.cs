using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLockStatementSyntax(LockStatementSyntax node)
        {
            var parts = new Parts(
                this.PrintSyntaxToken(node.LockKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else
            {
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}
