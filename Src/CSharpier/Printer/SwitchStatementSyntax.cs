using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchStatementSyntax(SwitchStatementSyntax node)
        {
            Doc sections = node.Sections.Count == 0
                ? " "
                : Docs.Concat(
                        Docs.Indent(
                            Docs.Concat(
                                Docs.HardLine,
                                Docs.Join(
                                    Docs.HardLine,
                                    node.Sections.Select(this.Print)
                                )
                            )
                        ),
                        Docs.HardLine
                    );
            return Docs.Concat(
                ExtraNewLines.Print(node),
                Docs.Group(
                    this.PrintSyntaxToken(
                        node.SwitchKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    SyntaxTokens.Print(node.OpenParenToken),
                    this.Print(node.Expression),
                    SyntaxTokens.Print(node.CloseParenToken),
                    Docs.Line,
                    SyntaxTokens.Print(node.OpenBraceToken),
                    sections,
                    SyntaxTokens.Print(node.CloseBraceToken)
                )
            );
        }
    }
}
