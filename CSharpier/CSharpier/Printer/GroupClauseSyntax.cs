using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGroupClauseSyntax(GroupClauseSyntax node)
        {
            return Concat(
                node.GroupKeyword.Text,
                String(" "),
                this.Print(node.GroupExpression),
                String(" "),
                node.ByKeyword.Text,
                String(" "),
                this.Print(node.ByExpression)
            );
        }
    }
}
