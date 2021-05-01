using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGotoStatementSyntax(GotoStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Doc.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.GotoKeyword),
                node.CaseOrDefaultKeyword.RawKind != 0 ? " " : Doc.Null,
                Token.Print(node.CaseOrDefaultKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
