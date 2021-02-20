using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDefaultConstraintSyntax(DefaultConstraintSyntax node)
        {
            return this.PrintSyntaxToken(node.DefaultKeyword);
        }
    }
}