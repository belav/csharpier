using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class UnsafeStatement
    {
        public static Doc Print(UnsafeStatementSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.UnsafeKeyword),
                Node.Print(node.Block)
            );
        }
    }
}
