using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementBindingExpression
{
    public static Doc Print(ElementBindingExpressionSyntax node, CSharpPrintingContext context)
    {
        return Node.Print(node.ArgumentList, context);
    }
}
