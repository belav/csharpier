using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGotoStatementSyntax(GotoStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(String(" "), this.Print(node.Expression)) : "";
            return Concat(
                node.GotoKeyword.Text,
                node.CaseOrDefaultKeyword.RawKind != 0 ? String(" ") : "",
                node.CaseOrDefaultKeyword.Text,
                expression,
                String(";")
            );
        }
    }
}
