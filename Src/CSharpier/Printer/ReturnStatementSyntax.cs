using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintReturnStatementSyntax(ReturnStatementSyntax node)
        {
            return Doc.Group(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.ReturnKeyword,
                    node.Expression != null ? " " : Doc.Null
                ),
                node.Expression != null
                    ? this.Print(node.Expression)
                    : Doc.Null,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
