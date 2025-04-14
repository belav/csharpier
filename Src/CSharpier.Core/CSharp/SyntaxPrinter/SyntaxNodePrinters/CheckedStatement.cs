using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class CheckedStatement
{
    public static Doc Print(CheckedStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(Token.Print(node.Keyword, context), Block.Print(node.Block, context));
    }
}
