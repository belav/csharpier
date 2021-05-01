using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefExpressionSyntax(RefExpressionSyntax node)
        {
            // TODO 1 should all " " turn into spaceIfNoPreviousComment? Maybe we just make a type for space and make it do that?
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.RefKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Expression)
            );
        }
    }
}
