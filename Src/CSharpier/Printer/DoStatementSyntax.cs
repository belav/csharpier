using CSharpier.DocTypes;
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
                SyntaxTokens.Print(node.OpenParenToken),
                this.Print(node.Condition),
                SyntaxTokens.Print(node.CloseParenToken),
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
