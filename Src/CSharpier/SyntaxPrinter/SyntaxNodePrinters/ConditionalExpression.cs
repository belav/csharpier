using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConditionalExpression
    {
        public static Doc Print(ConditionalExpressionSyntax node)
        {
            Doc[] contents =
            {
                Doc.Line,
                Token.PrintWithSuffix(node.QuestionToken, " "),
                Doc.Align(2, Node.Print(node.WhenTrue)),
                Doc.Line,
                Token.PrintWithSuffix(node.ColonToken, " "),
                Doc.Align(2, Node.Print(node.WhenFalse))
            };

            return Doc.Group(
                Node.Print(node.Condition),
                node.Parent is ConditionalExpressionSyntax or ArgumentSyntax
                    ? Doc.Align(2, contents)
                    : Doc.Indent(contents)
            );
        }
    }
}
