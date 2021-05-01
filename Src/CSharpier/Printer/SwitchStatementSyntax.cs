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
                : Doc.Concat(
                        Doc.Indent(
                            Doc.Concat(
                                Doc.HardLine,
                                Doc.Join(
                                    Doc.HardLine,
                                    node.Sections.Select(this.Print)
                                )
                            )
                        ),
                        Doc.HardLine
                    );
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Doc.Group(
                    this.PrintSyntaxToken(
                        node.SwitchKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    Token.Print(node.OpenParenToken),
                    this.Print(node.Expression),
                    Token.Print(node.CloseParenToken),
                    Doc.Line,
                    Token.Print(node.OpenBraceToken),
                    sections,
                    Token.Print(node.CloseBraceToken)
                )
            );
        }
    }
}
