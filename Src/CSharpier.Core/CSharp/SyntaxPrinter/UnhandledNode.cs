using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class UnhandledNode
{
    public static Doc Print(SyntaxNode node, PrintingContext context)
    {
        // full string includes comments/directives but also any whitespace, which we need to strip
        return node.ToFullString().Trim();
    }
}
