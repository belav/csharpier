using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitStackAllocArrayCreationExpression
{
    public static Doc Print(
        ImplicitStackAllocArrayCreationExpressionSyntax node,
        CSharpPrintingContext context
    )
    {
        return Doc.Concat(
            Token.Print(node.StackAllocKeyword, context),
            Token.Print(node.OpenBracketToken, context),
            Token.PrintWithSuffix(node.CloseBracketToken, " ", context),
            Node.Print(node.Initializer, context)
        );
    }
}
