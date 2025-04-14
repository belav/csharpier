using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class StackAllocArrayCreationExpression
{
    public static Doc Print(StackAllocArrayCreationExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.StackAllocKeyword, " ", context),
            Node.Print(node.Type, context),
            node.Initializer != null
                ? Doc.Concat(" ", InitializerExpression.Print(node.Initializer, context))
                : string.Empty
        );
    }
}
