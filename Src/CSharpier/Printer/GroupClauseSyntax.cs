using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGroupClauseSyntax(GroupClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(
                    node.GroupKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.GroupExpression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(
                    node.ByKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.ByExpression)
            );
        }
    }
}
