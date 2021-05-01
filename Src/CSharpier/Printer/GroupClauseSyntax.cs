using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGroupClauseSyntax(GroupClauseSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.GroupKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.GroupExpression),
                " ",
                this.PrintSyntaxToken(
                    node.ByKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.ByExpression)
            );
        }
    }
}
