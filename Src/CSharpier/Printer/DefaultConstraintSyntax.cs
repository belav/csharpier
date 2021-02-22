using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDefaultConstraintSyntax(DefaultConstraintSyntax node)
        {
            return this.PrintSyntaxToken(node.DefaultKeyword);
        }
    }
}
