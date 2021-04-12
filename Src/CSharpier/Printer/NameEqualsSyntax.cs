using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNameEqualsSyntax(NameEqualsSyntax node)
        {
            return Docs.Concat(
                this.Print(node.Name),
                Docs.SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(
                    node.EqualsToken,
                    afterTokenIfNoTrailing: " "
                )
            );
        }
    }
}
