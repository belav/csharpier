using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFixedStatementSyntax(FixedStatementSyntax node)
        {
            return Concat(
                node.FixedKeyword.Text,
                " ",
                "(",
                this.Print(node.Declaration),
                ")",
                this.Print(node.Statement)
            );
        }
    }
}
