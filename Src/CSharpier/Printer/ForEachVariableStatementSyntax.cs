using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachVariableStatementSyntax(
            ForEachVariableStatementSyntax node
        ) {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.AwaitKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.ForEachKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.OpenParenToken),
                this.Print(node.Variable),
                " ",
                this.PrintSyntaxToken(
                    node.InKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Expression),
                SyntaxTokens.Print(node.CloseParenToken),
                this.Print(node.Statement)
            );
        }
    }
}
