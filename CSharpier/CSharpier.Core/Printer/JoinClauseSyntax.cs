using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Concat(
                "join ",
                node.Identifier.Text,
                " in ",
                this.Print(node.InExpression),
                " on ",
                this.Print(node.LeftExpression),
                " equals ",
                this.Print(node.RightExpression),
                node.Into != null ? " into " + node.Into.Identifier.Text : ""
            );
        }
    }
}
