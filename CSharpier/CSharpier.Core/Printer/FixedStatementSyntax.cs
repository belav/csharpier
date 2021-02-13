using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFixedStatementSyntax(FixedStatementSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.FixedKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Declaration),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Statement)
            );
        }
    }
}
