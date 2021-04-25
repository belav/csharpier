using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintReturnStatementSyntax(ReturnStatementSyntax node)
        {
            return Docs.Group(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.ReturnKeyword,
                    node.Expression != null ? " " : Doc.Null
                ),
                node.Expression != null
                    ? this.Print(node.Expression)
                    : Docs.Null,
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
