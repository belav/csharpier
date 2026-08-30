using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedArraySizeExpression
{
    public static Doc Print(OmittedArraySizeExpressionSyntax node, CSharpPrintingContext context)
    {
        return Doc.Null;
    }
}
