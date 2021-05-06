using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SwitchExpression
    {
        public static Doc Print(SwitchExpressionSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.GoverningExpression),
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
                                    Node.Print(o.Pattern),
                                    " ",
                                    o.WhenClause != null
                                        ? Doc.Concat(Node.Print(o.WhenClause), " ")
                                        : Doc.Null,
                                    Token.Print(o.EqualsGreaterThanToken, " "),
                                    Node.Print(o.Expression)
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
