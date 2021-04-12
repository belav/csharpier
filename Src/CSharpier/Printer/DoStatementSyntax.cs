using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDoStatementSyntax(DoStatementSyntax node)
        {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.DoKeyword,
                    node.Statement is not BlockSyntax ? " " : Docs.Null
                ),
                SyntaxNodes.Print(node.Statement),
                Docs.HardLine,
                this.PrintSyntaxToken(
                    node.WhileKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Condition),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
