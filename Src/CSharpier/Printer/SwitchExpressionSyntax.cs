using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchExpressionSyntax(SwitchExpressionSyntax node)
        {
            return Concat(
                this.Print(node.GoverningExpression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.SwitchKeyword),
                HardLine,
                this.PrintSyntaxToken(node.OpenBraceToken),
                Group(
                    Indent(
                        Concat(
                            HardLine,
                            this.PrintSeparatedSyntaxList(
                                node.Arms,
                                o => Concat(
                                    this.Print(o.Pattern),
                                    SpaceIfNoPreviousComment,
                                    o.WhenClause != null
                                        ? Concat(
                                            this.Print(o.WhenClause),
                                            SpaceIfNoPreviousComment
                                        )
                                        : Doc.Null,
                                    this.PrintSyntaxToken(
                                        o.EqualsGreaterThanToken,
                                        " "
                                    ),
                                    this.Print(o.Expression)
                                ),
                                HardLine
                            )
                        )
                    ),
                    HardLine
                ),
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
        }
    }
}
