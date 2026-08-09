using System.Diagnostics.CodeAnalysis;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ContinueStatement
{
    [Experimental("RSEXPERIMENTAL006")]
    public static Doc Print(ContinueStatementSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(
                node.ContinueKeyword,
                node.Name != null ? " " : Doc.Null,
                context
            ),
            node.Name != null ? Node.Print(node.Name, context) : Doc.Null,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
