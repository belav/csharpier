using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SimpleLambdaExpression
    {
        public static Doc Print(SimpleLambdaExpressionSyntax node)
        {
            return Doc.Group(
                Modifiers.Print(node.Modifiers),
                Node.Print(node.Parameter),
                " ",
                Token.Print(node.ArrowToken),
                node.Body
                    is BlockSyntax blockSyntax
                    ? Block.Print(blockSyntax)
                    : Doc.Indent(Doc.Line, Node.Print(node.Body))
            );
        }
    }
}
