using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AssignmentExpression
    {
        public static Doc Print(AssignmentExpressionSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Left),
                " ",
                Token.Print(node.OperatorToken),
                node.Right is QueryExpressionSyntax
                    ? Doc.Indent(Doc.Line, Node.Print(node.Right))
                    : Doc.Concat(" ", Node.Print(node.Right))
            );
        }
    }
}
