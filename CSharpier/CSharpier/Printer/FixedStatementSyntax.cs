using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFixedStatementSyntax(FixedStatementSyntax node)
        {
            return Concat(
                String(node.FixedKeyword.Text),
                String(" "),
                String("("),
                this.Print(node.Declaration),
                String(")"),
                this.Print(node.Statement)
            );
        }
    }
}
