using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ArrayCreationExpression
    {
        public static Doc Print(ArrayCreationExpressionSyntax node)
        {
            return Doc.Group(
                Token.Print(node.NewKeyword, " "),
                Node.Print(node.Type),
                node.Initializer != null
                    ? Doc.Concat(Doc.Line, Node.Print(node.Initializer))
                    : Doc.Null
            );
        }
    }
}
