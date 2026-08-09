using System.Diagnostics.CodeAnalysis;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class BreakStatement
{
    [Experimental("RSEXPERIMENTAL006")]
    public static Doc Print(BreakStatementSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.BreakKeyword, node.Name != null ? " " : Doc.Null, context),
            node.Name != null ? Node.Print(node.Name, context) : Doc.Null,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
