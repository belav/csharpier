using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCheckedStatementSyntax(CheckedStatementSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.Keyword), this.PrintBlockSyntax(node.Block));
        }
    }
}
