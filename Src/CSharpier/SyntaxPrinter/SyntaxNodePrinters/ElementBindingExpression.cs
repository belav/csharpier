using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementBindingExpression
{
    public static Doc Print(ElementBindingExpressionSyntax node)
    {
        return Node.Print(node.ArgumentList);
    }
}
