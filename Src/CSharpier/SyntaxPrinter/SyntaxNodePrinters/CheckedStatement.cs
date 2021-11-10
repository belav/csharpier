using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class CheckedStatement
    {
        public static Doc Print(CheckedStatementSyntax node)
        {
            return Doc.Concat(Token.Print(node.Keyword), Block.Print(node.Block));
        }
    }
}
