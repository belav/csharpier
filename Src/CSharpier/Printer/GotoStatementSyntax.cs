using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGotoStatementSyntax(GotoStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Docs.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.GotoKeyword),
                node.CaseOrDefaultKeyword.RawKind != 0
                    ? Docs.SpaceIfNoPreviousComment
                    : Docs.Null,
                this.PrintSyntaxToken(node.CaseOrDefaultKeyword),
                expression,
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
