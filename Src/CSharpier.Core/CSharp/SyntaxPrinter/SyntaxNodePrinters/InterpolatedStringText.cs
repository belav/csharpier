using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringText
{
    public static Doc Print(InterpolatedStringTextSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.TextToken, context);
    }
}
