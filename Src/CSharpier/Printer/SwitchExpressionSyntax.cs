using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchExpressionSyntax(SwitchExpressionSyntax node)
        {
            return Doc.Concat(
                this.Print(node.GoverningExpression),
                " ",
                Token.Print(node.SwitchKeyword),
                Doc.HardLine,
                Token.Print(node.OpenBraceToken),
                Doc.Group(
                    Doc.Indent(
                        Doc.HardLine,
                        SeparatedSyntaxList.Print(
                            node.Arms,
                            o =>
                                Doc.Concat(
                                    this.Print(o.Pattern),
                                    " ",
                                    o.WhenClause != null
                                        ? Doc.Concat(
                                                this.Print(o.WhenClause),
                                                " "
                                            )
                                        : Doc.Null,
                                    this.PrintSyntaxToken(
                                        o.EqualsGreaterThanToken,
                                        " "
                                    ),
                                    this.Print(o.Expression)
                                ),
                            Doc.HardLine
                        )
                    ),
                    Doc.HardLine
                ),
                Token.Print(node.CloseBraceToken)
            );
        }
    }
}
