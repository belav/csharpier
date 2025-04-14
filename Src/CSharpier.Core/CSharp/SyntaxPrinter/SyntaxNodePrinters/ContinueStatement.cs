using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ContinueStatement
{
    public static Doc Print(ContinueStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.ContinueKeyword, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
