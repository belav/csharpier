using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedArraySizeExpression
{
    public static Doc Print(OmittedArraySizeExpressionSyntax node)
    {
        return Doc.Null;
    }
}
