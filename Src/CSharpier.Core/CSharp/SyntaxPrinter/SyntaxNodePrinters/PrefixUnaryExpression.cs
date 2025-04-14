using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class PrefixUnaryExpression
{
    public static Doc Print(PrefixUnaryExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.OperatorToken, context),
            Node.Print(node.Operand, context)
        );
    }
}
