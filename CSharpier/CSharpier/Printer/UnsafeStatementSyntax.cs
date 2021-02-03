using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnsafeStatementSyntax(UnsafeStatementSyntax node)
        {
            return Concat(String(node.UnsafeKeyword.Text), this.Print(node.Block));
        }
    }
}
