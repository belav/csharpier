using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForStatementSyntax(ForStatementSyntax node)
        {
            var parts = new Parts(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.ForKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken));
            var innerGroup = new Parts();
            innerGroup.Push(SoftLine);
            if (node.Declaration != null)
            {
                innerGroup.Push(
                    this.PrintVariableDeclarationSyntax(node.Declaration));
            }
            innerGroup.Push(
                this.PrintSeparatedSyntaxList(
                    node.Initializers,
                    this.Print,
                    " "));
            innerGroup.Push(this.PrintSyntaxToken(node.FirstSemicolonToken));
            if (node.Condition != null)
            {
                innerGroup.Push(Line, this.Print(node.Condition));
            }
            else
            {
                innerGroup.Push(SoftLine);
            }

            innerGroup.Push(this.PrintSyntaxToken(node.SecondSemicolonToken));
            if (node.Incrementors.Any())
            {
                innerGroup.Push(Line);
            }
            else
            {
                innerGroup.Push(SoftLine);
            }
            innerGroup.Push(
                this.PrintSeparatedSyntaxList(
                    node.Incrementors,
                    this.Print,
                    Line));
            parts.Push(Group(Indent(innerGroup.ToArray())));
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
