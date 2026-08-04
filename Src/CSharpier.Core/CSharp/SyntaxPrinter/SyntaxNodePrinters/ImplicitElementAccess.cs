using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitElementAccess
{
    public static Doc Print(ImplicitElementAccessSyntax node, CSharpPrintingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
