using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CheckedStatement
    {
        public static Doc Print(CheckedStatementSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Block.Print(node.Block)
            );
        }
    }
}
