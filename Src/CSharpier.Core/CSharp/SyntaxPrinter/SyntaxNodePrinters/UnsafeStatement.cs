using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class UnsafeStatement
{
    public static Doc Print(UnsafeStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.UnsafeKeyword, context),
            Node.Print(node.Block, context)
        );
    }
}
