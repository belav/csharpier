using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThrowStatementSyntax(ThrowStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Docs.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Docs.Concat(
                ExtraNewLines.Print(node),
                SyntaxTokens.Print(node.ThrowKeyword),
                expression,
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
