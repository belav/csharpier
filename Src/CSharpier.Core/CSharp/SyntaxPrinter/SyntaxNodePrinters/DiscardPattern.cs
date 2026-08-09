using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardPattern
{
    public static Doc Print(DiscardPatternSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.UnderscoreToken, context);
    }
}
