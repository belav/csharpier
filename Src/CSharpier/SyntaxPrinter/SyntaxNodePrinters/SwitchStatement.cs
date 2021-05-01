using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SwitchStatement
    {
        public static Doc Print(SwitchStatementSyntax node)
        {
            Doc sections = node.Sections.Count == 0
                ? " "
                : Doc.Concat(
                        Doc.Indent(
                            Doc.Concat(
                                Doc.HardLine,
                                Doc.Join(
                                    Doc.HardLine,
                                    node.Sections.Select(Node.Print)
                                )
                            )
                        ),
                        Doc.HardLine
                    );
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Doc.Group(
                    Token.Print(node.SwitchKeyword, " "),
                    Token.Print(node.OpenParenToken),
                    Node.Print(node.Expression),
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
