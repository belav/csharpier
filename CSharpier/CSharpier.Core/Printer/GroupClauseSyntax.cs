using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintGroupClauseSyntax(GroupClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.GroupKeyword, " "),
                this.Print(node.GroupExpression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.ByKeyword, " "),
                this.Print(node.ByExpression)
            );
        }
    }
}
