using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RecursivePattern
    {
        public static Doc Print(RecursivePatternSyntax node)
        {
            return Doc.Concat(
                node.Type != null ? Doc.Concat(Node.Print(node.Type), " ") : Doc.Null,
                node.PositionalPatternClause != null
                    ? Doc.Concat(
                            Token.Print(node.PositionalPatternClause.OpenParenToken),
                            SeparatedSyntaxList.Print(
                                node.PositionalPatternClause.Subpatterns,
                                subpatternNode =>
                                    Doc.Concat(
                                        subpatternNode.NameColon != null
                                            ? NameColon.Print(subpatternNode.NameColon)
                                            : Doc.Null,
                                        Node.Print(subpatternNode.Pattern)
                                    ),
                                " "
                            ),
                            Token.Print(node.PositionalPatternClause.CloseParenToken)
                        )
                    : Doc.Null,
                node.PropertyPatternClause != null
                    ? Doc.Concat(
                            Token.PrintWithSuffix(node.PropertyPatternClause.OpenBraceToken, " "),
                            SeparatedSyntaxList.Print(
                                node.PropertyPatternClause.Subpatterns,
                                subpatternNode =>
                                    Doc.Concat(
                                        subpatternNode.NameColon != null
                                            ? NameColon.Print(subpatternNode.NameColon)
                                            : Doc.Null,
                                        Node.Print(subpatternNode.Pattern)
                                    ),
                                " "
                            ),
                            node.PropertyPatternClause.Subpatterns.Any() ? " " : Doc.Null,
                            Token.Print(node.PropertyPatternClause.CloseBraceToken)
                        )
                    : Doc.Null,
                node.Designation != null ? Doc.Concat(" ", Node.Print(node.Designation)) : Doc.Null
            );
        }
    }
}
