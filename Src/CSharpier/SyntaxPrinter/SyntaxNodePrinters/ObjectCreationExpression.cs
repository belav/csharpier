using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ObjectCreationExpression
    {
        public static Doc Print(ObjectCreationExpressionSyntax node)
        {
            return Doc.Group(
                Token.Print(node.NewKeyword, " "),
                Node.Print(node.Type),
                node.ArgumentList != null
                    ? ArgumentList.Print(node.ArgumentList)
                    : string.Empty,
                node.Initializer != null
                    ? InitializerExpression.Print(node.Initializer)
                    : string.Empty
            );
        }
    }
}
