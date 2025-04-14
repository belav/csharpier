using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DeclarationExpression
{
    public static Doc Print(DeclarationExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Type, context),
            " ",
            Node.Print(node.Designation, context)
        );
    }
}
