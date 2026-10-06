using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WithElement
{
    public static Doc Print(WithElementSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.WithKeyword, context),
            ArgumentList.Print(node.ArgumentList, context)
        );
    }
}
