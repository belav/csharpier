using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DefaultConstraint
    {
        // TODO 0 look into things that aren't covered by unit tests
        public static Doc Print(DefaultConstraintSyntax node)
        {
            return Token.Print(node.DefaultKeyword);
        }
    }
}
