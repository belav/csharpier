using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EmptyStatement
    {
        public static Doc Print(EmptyStatementSyntax node)
        {
            return Token.Print(node.SemicolonToken);
        }
    }
}
