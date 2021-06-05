using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DefaultConstraint
    {
        public static Doc Print(DefaultConstraintSyntax node)
        {
            return Token.Print(node.DefaultKeyword);
        }
    }
}
