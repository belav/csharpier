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
                Token.PrintWithSuffix(
                    node.DoKeyword,
                    node.Statement
                        is not BlockSyntax ? " " : Doc.Null
                ),
                Node.Print(node.Statement),
                Doc.HardLine,
                Token.PrintWithSuffix(node.WhileKeyword, " "),
                Token.Print(node.OpenParenToken),
                Doc.Group(Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)), Doc.SoftLine),
                Token.Print(node.CloseParenToken),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
