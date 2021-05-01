using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ReturnStatement
    {
        public static Doc Print(ReturnStatementSyntax node)
        {
            return Doc.Group(
                ExtraNewLines.Print(node),
                Token.Print(
                    node.ReturnKeyword,
                    node.Expression != null ? " " : Doc.Null
                ),
                node.Expression != null
                    ? Node.Print(node.Expression)
                    : Doc.Null,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
