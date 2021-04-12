using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchExpressionSyntax(SwitchExpressionSyntax node)
        {
            return Docs.Concat(
                this.Print(node.GoverningExpression),
                Docs.SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.SwitchKeyword),
                Docs.HardLine,
                this.PrintSyntaxToken(node.OpenBraceToken),
                Docs.Group(
                    Docs.Indent(
                        Docs.HardLine,
                        this.PrintSeparatedSyntaxList(
                            node.Arms,
                            o =>
                                Docs.Concat(
                                    this.Print(o.Pattern),
                                    Docs.SpaceIfNoPreviousComment,
                                    o.WhenClause != null
                                        ? Docs.Concat(
                                                this.Print(o.WhenClause),
                                                Docs.SpaceIfNoPreviousComment
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
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
        }
    }
}
