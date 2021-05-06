using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BreakStatement
    {
        public static Doc Print(BreakStatementSyntax node)
        {
            return Doc.Concat(Token.Print(node.BreakKeyword), Token.Print(node.SemicolonToken));
        }
    }
}
