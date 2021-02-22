using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintYieldStatementSyntax(YieldStatementSyntax node)
        {
            var expression = node.Expression != null
                ? Concat(" ", this.Print(node.Expression))
                : "";
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.YieldKeyword, " "),
                this.PrintSyntaxToken(node.ReturnOrBreakKeyword),
                expression,
                this.PrintSyntaxToken(node.SemicolonToken));
        }
    }
}
