using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BreakStatement
{
    public static Doc Print(BreakStatementSyntax node)
    {
        return Doc.Concat(Token.Print(node.BreakKeyword), Token.Print(node.SemicolonToken));
    }
}
