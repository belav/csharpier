using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForStatementSyntax(ForStatementSyntax node)
        {
            var parts = new Parts(node.ForKeyword.Text, " (");
            if (node.Declaration != null)
            {
                parts.Push(
                    this.PrintVariableDeclarationSyntax(node.Declaration),
                    "; "
                );
            }
            else
            {
                parts.Add(String(";"));
            }

            if (node.Condition != null)
            {
                parts.Push(this.Print(node.Condition), String("; "));
            }
            else
            {
                parts.Add(String(";"));
            }

            parts.Push(Join(String(", "), node.Incrementors.Select(this.Print)), String(")"));
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