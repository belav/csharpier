using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstantPattern
{
    public static Doc Print(ConstantPatternSyntax node, PrintingContext context)
    {
        return Node.Print(node.Expression, context);
    }
}
