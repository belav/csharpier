using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IsPatternExpression
    {
        public static Doc Print(IsPatternExpressionSyntax node)
        {
            if (node.Parent is not (IfStatementSyntax or ParenthesizedExpressionSyntax))
            {
                return Doc.Group(
                    Node.Print(node.Expression),
                    Doc.Indent(
                        Doc.Line,
                        Token.Print(node.IsKeyword),
                        node.Pattern is RecursivePatternSyntax { Type: null } ? Doc.Null : " ",
                        Node.Print(node.Pattern)
                    )
                );
            }

            if (node.Pattern is not RecursivePatternSyntax recursivePattern)
            {
                return Doc.Group(
                    Node.Print(node.Expression),
                    Doc.Line,
                    Token.Print(node.IsKeyword),
                    " ",
                    Node.Print(node.Pattern)
                );
            }

            if (recursivePattern.Type is null)
            {
                return Doc.Group(
                    Node.Print(node.Expression),
                    Doc.Line,
                    Token.Print(node.IsKeyword),
                    RecursivePattern.Print(recursivePattern)
                );
            }

            return Doc.Group(
                Doc.Group(
                    Node.Print(node.Expression),
                    Doc.Line,
                    Token.Print(node.IsKeyword),
                    " ",
                    Node.Print(recursivePattern.Type)
                ),
                RecursivePattern.PrintWithOutType(recursivePattern)
            );
        }
    }
}
