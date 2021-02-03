using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForStatementSyntax(ForStatementSyntax node)
        {
            var parts = new Parts(node.ForKeyword.Text, " (");
            if (NotNull(node.Declaration))
            {
                parts.Push(
                    this.PrintVariableDeclarationSyntax(node.Declaration),
                    "; "
                );
            }
            else
            {
                parts.Push(String(";"));
            }

            if (NotNull(node.Condition))
            {
                parts.Push(this.Print(node.Condition), String("; "));
            }
            else
            {
                parts.Push(String(";"));
            }

            parts.Push(Join(String(", "), node.Incrementors.Select(this.Print)), String(")"));
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