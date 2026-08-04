using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardDesignation
{
    public static Doc Print(DiscardDesignationSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.UnderscoreToken, context);
    }
}
