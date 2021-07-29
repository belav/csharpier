using CSharpier.DocTypes;
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
                                Doc.Group(
                                    Node.Print(o.Pattern),
                                    o.WhenClause != null
                                        ? Node.Print(o.WhenClause)
                                        : Doc.Null,
                                    // use align 2 here to make sure that the => never lines up with statements above it
                                    // it makes this more readable for big ugly switch expressions
                                    Doc.Align(
                                        2,
                                        Doc.Concat(
                                            Doc.Line,
                                            Token.PrintWithSuffix(o.EqualsGreaterThanToken, " "),
                                            Node.Print(o.Expression)
                                        )
                                    )
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
