using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayCreationExpression
{
    public static Doc Print(ArrayCreationExpressionSyntax node, CSharpPrintingContext context)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.NewKeyword, " ", context),
            Node.Print(node.Type, context),
            InitializerExpression.PrintOptionalWithLine(node.Initializer, context)
        );
    }
}
