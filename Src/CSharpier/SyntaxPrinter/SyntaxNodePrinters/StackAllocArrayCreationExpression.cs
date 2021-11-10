using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class StackAllocArrayCreationExpression
    {
        public static Doc Print(StackAllocArrayCreationExpressionSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.StackAllocKeyword, " "),
                Node.Print(node.Type),
                node.Initializer != null
                  ? Doc.Concat(" ", InitializerExpression.Print(node.Initializer))
                  : string.Empty
            );
        }
    }
}
