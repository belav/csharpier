using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 0 look into things that aren't covered by unit tests
        private Doc PrintDefaultConstraintSyntax(DefaultConstraintSyntax node)
        {
            return this.PrintSyntaxToken(node.DefaultKeyword);
        }
    }
}
