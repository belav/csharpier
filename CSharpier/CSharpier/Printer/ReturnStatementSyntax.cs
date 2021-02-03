using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintReturnStatementSyntax(ReturnStatementSyntax node)
        {
            if (node.Expression == null) {
                return node.ReturnKeyword.Text + String(";");
            }
            return Concat(node.ReturnKeyword.Text, String(" "), this.Print(node.Expression), String(";"));
        }
    }
}
