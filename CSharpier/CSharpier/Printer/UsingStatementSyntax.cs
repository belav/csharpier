using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var parts = new Parts(
                String(node.UsingKeyword.Text),
                String(" ("),
                node.Declaration != null ? this.PrintVariableDeclarationSyntax(node.Declaration) : "",
                node.Expression != null ? this.Print(node.Expression) : "",
                String(")")
            );
            var statement = this.Print(node.Statement);
            if (node.Statement is UsingStatementSyntax) {
                parts.Push(HardLine, statement);
            } else if (node.Statement is BlockSyntax) {
                parts.Add(statement);
            } else {
                parts.Add(Indent(Concat(HardLine, statement)));
            }
            return Concat(parts);
        }
    }
}
