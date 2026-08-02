using System.Diagnostics.CodeAnalysis;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WithElement
{
    [Experimental("RSEXPERIMENTAL006")]
    public static Doc Print(WithElementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.WithKeyword, context),
            ArgumentList.Print(node.ArgumentList, context)
        );
    }
}
