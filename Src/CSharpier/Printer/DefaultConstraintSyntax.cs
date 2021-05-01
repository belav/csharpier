using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 0 look into things that aren't covered by unit tests
        private Doc PrintDefaultConstraintSyntax(DefaultConstraintSyntax node)
        {
            return Token.Print(node.DefaultKeyword);
        }
    }
}
