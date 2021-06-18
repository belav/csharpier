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
                Assignment.PrintRhs(node.Right)
            );
        }
    }
}
