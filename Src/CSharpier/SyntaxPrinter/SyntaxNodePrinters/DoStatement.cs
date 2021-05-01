using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DoStatement
    {
        public static Doc Print(DoStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(
                    node.DoKeyword,
                    node.Statement is not BlockSyntax ? " " : Doc.Null
                ),
                Node.Print(node.Statement),
                Doc.HardLine,
                Token.Print(node.WhileKeyword, " "),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Condition),
                Token.Print(node.CloseParenToken),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
