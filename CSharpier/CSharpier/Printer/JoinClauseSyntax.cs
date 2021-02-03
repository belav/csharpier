using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Concat(
                String("join "),
                node.Identifier.Text,
                String(" in "),
                this.Print(node.InExpression),
                String(" on "),
                this.Print(node.LeftExpression),
                String(" equals "),
                this.Print(node.RightExpression),
                node.Into != null ? " into " + node.Into.Identifier.Text : ""
            );
        }
    }
}
