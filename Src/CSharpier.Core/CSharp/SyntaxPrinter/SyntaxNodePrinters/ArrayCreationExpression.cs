using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayCreationExpression
{
    public static Doc Print(ArrayCreationExpressionSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.NewKeyword, " ", context),
            Node.Print(node.Type, context),
            node.Initializer != null
                ? Doc.Concat(Doc.Line, InitializerExpression.Print(node.Initializer, context))
                : Doc.Null
        );
    }
}
