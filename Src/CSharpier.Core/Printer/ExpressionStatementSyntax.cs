using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintExpressionStatementSyntax(ExpressionStatementSyntax node)
        {
            return Concat(this.PrintExtraNewLines(node),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}