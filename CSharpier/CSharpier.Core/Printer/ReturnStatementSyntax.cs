using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintReturnStatementSyntax(ReturnStatementSyntax node)
        {
            if (node.Expression == null) {
                return node.ReturnKeyword.Text + ";";
            }
            return Concat(node.ReturnKeyword.Text, " ", this.Print(node.Expression), ";");
        }
    }
}
