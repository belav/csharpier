using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThrowStatementSyntax(ThrowStatementSyntax node)
        {
            var expression = node.Expression != null
                ? Concat(" ", this.Print(node.Expression))
                : "";
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.ThrowKeyword),
                expression,
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
