using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class DefaultConstraint
    {
        public static Doc Print(DefaultConstraintSyntax node)
        {
            return Token.Print(node.DefaultKeyword);
        }
    }
}
