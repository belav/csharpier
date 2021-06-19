using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ParenthesizedExpression
    {
        public static Doc Print(ParenthesizedExpressionSyntax node)
        {
            return Doc.Group(
                Token.Print(node.OpenParenToken),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression)),
                Doc.SoftLine,
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
