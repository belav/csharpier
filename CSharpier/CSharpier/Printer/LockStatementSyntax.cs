using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLockStatementSyntax(LockStatementSyntax node)
        {
            var parts = new Parts(node.LockKeyword.Text, String(" "), String("("), this.Print(node.Expression), String(")"));
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Add(statement);
            }
            else
            {
                parts.Add(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}