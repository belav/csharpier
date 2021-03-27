using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchStatementSyntax(SwitchStatementSyntax node)
        {
            var sections = node.Sections.Count == 0
                ? " "
                : Concat(
                    Indent(
                        Concat(
                            HardLine,
                            Join(HardLine, node.Sections.Select(this.Print))
                        )
                    ),
                    HardLine
                );
            return Group(
                this.PrintSyntaxToken(node.SwitchKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.CloseParenToken),
                Line,
                this.PrintSyntaxToken(node.OpenBraceToken),
                sections,
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
        }
    }
}
