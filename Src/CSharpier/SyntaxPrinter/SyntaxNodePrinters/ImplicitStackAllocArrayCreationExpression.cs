using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ImplicitStackAllocArrayCreationExpression
    {
        public static Doc Print(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.StackAllocKeyword),
                Token.Print(node.OpenBracketToken),
                Token.PrintWithSuffix(node.CloseBracketToken, " "),
                Node.Print(node.Initializer)
            );
        }
    }
}
