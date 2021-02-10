using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var parts = new Parts(
                node.UsingKeyword.Text,
                " (",
                node.Declaration != null ? this.PrintVariableDeclarationSyntax(node.Declaration) : "",
                node.Expression != null ? this.Print(node.Expression) : "",
                ")"
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
