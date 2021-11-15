using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PrefixUnaryExpression
{
    public static Doc Print(PrefixUnaryExpressionSyntax node)
    {
        return Doc.Concat(Token.Print(node.OperatorToken), Node.Print(node.Operand));
    }
}
