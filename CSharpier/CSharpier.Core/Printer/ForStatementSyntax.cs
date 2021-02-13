using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintForStatementSyntax(ForStatementSyntax node)
        {
            var parts = new Parts(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.ForKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken));
            if (node.Declaration != null)
            {
                parts.Push(this.PrintVariableDeclarationSyntax(node.Declaration));
            }
            parts.Push(this.PrintSyntaxToken(node.FirstSemicolonToken));
            if (node.Condition != null)
            {
                parts.Push(SpaceIfNoPreviousComment);
                parts.Push(this.Print(node.Condition));
            }
            parts.Push(this.PrintSyntaxToken(node.SecondSemicolonToken));
            if (node.Incrementors.Any())
            {
                parts.Push(SpaceIfNoPreviousComment);
            }
            parts.Push(this.PrintSeparatedSyntaxList(node.Incrementors, this.Print, Line));
            parts.Push(this.PrintSyntaxToken(node.CloseParenToken));
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else
            {
                // TODO 1 force braces? we do in if and else
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}