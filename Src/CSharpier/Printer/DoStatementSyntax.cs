using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDoStatementSyntax(DoStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.DoKeyword,
                    node.Statement is not BlockSyntax ? " " : Doc.Null
                ),
                Node.Print(node.Statement),
                Doc.HardLine,
                this.PrintSyntaxToken(
                    node.WhileKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Token.Print(node.OpenParenToken),
                this.Print(node.Condition),
                Token.Print(node.CloseParenToken),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
