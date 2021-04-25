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
            return Docs.Concat(
                this.Print(node.GoverningExpression),
                " ",
                SyntaxTokens.Print(node.SwitchKeyword),
                Docs.HardLine,
                SyntaxTokens.Print(node.OpenBraceToken),
                Docs.Group(
                    Docs.Indent(
                        Docs.HardLine,
                        SeparatedSyntaxList.Print(
                            node.Arms,
                            o =>
                                Docs.Concat(
                                    this.Print(o.Pattern),
                                    " ",
                                    o.WhenClause != null
                                        ? Docs.Concat(
                                                this.Print(o.WhenClause),
                                                " "
                                            )
                                        : Docs.Null,
                                    this.PrintSyntaxToken(
                                        o.EqualsGreaterThanToken,
                                        " "
                                    ),
                                    this.Print(o.Expression)
                                ),
                            Docs.HardLine
                        )
                    ),
                    Docs.HardLine
                ),
                SyntaxTokens.Print(node.CloseBraceToken)
            );
        }
    }
}
