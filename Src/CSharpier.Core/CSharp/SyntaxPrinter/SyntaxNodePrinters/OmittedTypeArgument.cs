using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedTypeArgument
{
    public static Doc Print(OmittedTypeArgumentSyntax node, CSharpPrintingContext context)
    {
        return Doc.Null;
    }
}
