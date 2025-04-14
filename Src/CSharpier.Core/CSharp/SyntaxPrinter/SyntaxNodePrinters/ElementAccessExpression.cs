using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElementAccessExpression
{
    public static Doc Print(ElementAccessExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            Node.Print(node.ArgumentList, context)
        );
    }
}
