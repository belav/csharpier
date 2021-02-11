using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintLockStatementSyntax(LockStatementSyntax node)
        {
            var parts = new Parts(node.LockKeyword.Text, " ", "(", this.Print(node.Expression), ")");
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