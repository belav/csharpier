using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ContinueStatement
    {
        public static Doc Print(ContinueStatementSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.ContinueKeyword),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
