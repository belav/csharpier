using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WithExpression
{
    public static Doc Print(WithExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            " ",
            Token.PrintWithSuffix(node.WithKeyword, Doc.Line, context),
            Node.Print(node.Initializer, context)
        );
    }
}
