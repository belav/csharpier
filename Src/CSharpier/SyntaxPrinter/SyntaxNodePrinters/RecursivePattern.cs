using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RecursivePattern
    {
        public static Doc Print(RecursivePatternSyntax node)
        {
            var result = new List<Doc>();
            if (node.Type != null)
            {
                result.Add(Node.Print(node.Type));
            }

            if (node.PositionalPatternClause != null)
            {
                result.Add(
                    Doc.SoftLine,
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
                );
            }

            if (node.PropertyPatternClause != null)
            {
                result.Add(
                    node.Parent switch
                    {
                        IsPatternExpressionSyntax => Doc.Line,
                        SwitchExpressionArmSyntax => Doc.Null,
                        _ => Doc.SoftLine
                    },
                    Token.Print(node.PropertyPatternClause.OpenBraceToken),
                    Doc.Indent(
                        node.PropertyPatternClause.Subpatterns.Any() ? Doc.Line : Doc.Null,
                        SeparatedSyntaxList.Print(
                            node.PropertyPatternClause.Subpatterns,
                            subpatternNode =>
                                Doc.Group(
                                    subpatternNode.NameColon != null
                                        ? NameColon.Print(subpatternNode.NameColon)
                                        : Doc.Null,
                                    Node.Print(subpatternNode.Pattern)
                                ),
                            Doc.Line
                        )
                    ),
                    Doc.Line,
                    Token.Print(node.PropertyPatternClause.CloseBraceToken)
                );
            }

            if (node.Designation != null)
            {
                result.Add(" ", Node.Print(node.Designation));
            }

            return Doc.Concat(result);
        }
    }
}
