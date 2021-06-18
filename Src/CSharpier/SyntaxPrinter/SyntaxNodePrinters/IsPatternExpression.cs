using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IsPatternExpression
    {
        public static Doc Print(IsPatternExpressionSyntax node)
        {
            return Doc.Group(
                Node.Print(node.Expression),
                Doc.Indent(
                    Doc.Line,
                    Token.PrintWithSuffix(node.IsKeyword, " "),
                    Doc.Indent(Node.Print(node.Pattern))
                )
            );
        }
    }
}
