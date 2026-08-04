using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class PredefinedType
{
    public static Doc Print(PredefinedTypeSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.Keyword, context);
    }
}
