using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintElseClauseSyntax(ElseClauseSyntax node)
        {
            var parts = new Parts();
            parts.Push(node.ElseKeyword.Text);
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else if (node.Statement is IfStatementSyntax)
            {
                parts.Push(String(" "), statement);
            }
            else
            {
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}