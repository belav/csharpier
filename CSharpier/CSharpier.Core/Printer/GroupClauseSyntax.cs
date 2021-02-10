using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintGroupClauseSyntax(GroupClauseSyntax node)
        {
            return Concat(
                node.GroupKeyword.Text,
                " ",
                this.Print(node.GroupExpression),
                " ",
                node.ByKeyword.Text,
                " ",
                this.Print(node.ByExpression)
            );
        }
    }
}
