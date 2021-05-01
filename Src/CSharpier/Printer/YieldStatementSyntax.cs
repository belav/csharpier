using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintYieldStatementSyntax(YieldStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Doc.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.YieldKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Token.Print(node.ReturnOrBreakKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
