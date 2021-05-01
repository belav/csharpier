using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBreakStatementSyntax(BreakStatementSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.BreakKeyword),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
