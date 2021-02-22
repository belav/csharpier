using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachVariableStatementSyntax(
            ForEachVariableStatementSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.AwaitKeyword, " "),
                this.PrintSyntaxToken(node.ForEachKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Variable),
                " ",
                this.PrintSyntaxToken(node.InKeyword, " "),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.CloseParenToken),
                this.Print(node.Statement));
        }
    }
}
