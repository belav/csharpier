using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintYieldStatementSyntax(YieldStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Docs.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.YieldKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.ReturnOrBreakKeyword),
                expression,
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
