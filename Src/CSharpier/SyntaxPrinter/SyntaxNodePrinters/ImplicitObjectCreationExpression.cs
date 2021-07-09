using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ImplicitObjectCreationExpression
    {
        public static Doc Print(ImplicitObjectCreationExpressionSyntax node)
        {
            return Doc.Group(
                Token.Print(node.NewKeyword),
                ArgumentList.Print(node.ArgumentList),
                node.Initializer != null
                    ? Doc.Concat(Doc.Line, Node.Print(node.Initializer))
                    : Doc.Null
            );
        }
    }
}
