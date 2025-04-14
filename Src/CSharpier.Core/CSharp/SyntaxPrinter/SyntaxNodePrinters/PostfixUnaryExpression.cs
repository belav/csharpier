using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class PostfixUnaryExpression
{
    public static Doc Print(PostfixUnaryExpressionSyntax node, PrintingContext context)
    {
        if (
            node.Kind() is SyntaxKind.SuppressNullableWarningExpression
            && node.Operand is InvocationExpressionSyntax
        )
        {
            return InvocationExpression.PrintMemberChain(node, context);
        }

        return Doc.Concat(
            Node.Print(node.Operand, context),
            Token.Print(node.OperatorToken, context)
        );
    }
}
