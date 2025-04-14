using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DefaultConstraint
{
    public static Doc Print(DefaultConstraintSyntax node, PrintingContext context)
    {
        return Token.Print(node.DefaultKeyword, context);
    }
}
