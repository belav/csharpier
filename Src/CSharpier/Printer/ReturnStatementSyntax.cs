using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintReturnStatementSyntax(ReturnStatementSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.ReturnKeyword,
                    node.Expression != null ? " " : null
                ),
                node.Expression != null ? this.Print(node.Expression) : null,
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
