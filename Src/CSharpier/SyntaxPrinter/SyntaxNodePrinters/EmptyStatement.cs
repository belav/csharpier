using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class EmptyStatement
    {
        public static Doc Print(EmptyStatementSyntax node)
        {
            return Token.Print(node.SemicolonToken);
        }
    }
}
