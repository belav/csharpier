using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintGotoStatementSyntax(GotoStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(" ", this.Print(node.Expression)) : "";
            return Concat(
                node.GotoKeyword.Text,
                node.CaseOrDefaultKeyword.RawKind != 0 ? " " : "",
                node.CaseOrDefaultKeyword.Text,
                expression,
                ";"
            );
        }
    }
}
