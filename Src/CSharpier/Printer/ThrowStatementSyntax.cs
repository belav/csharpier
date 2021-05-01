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
                ? Doc.Concat(" ", this.Print(node.Expression))
                : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.ThrowKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
