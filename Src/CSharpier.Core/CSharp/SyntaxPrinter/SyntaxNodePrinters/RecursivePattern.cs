using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class RecursivePattern
{
    public static Doc PrintWithOutType(RecursivePatternSyntax node, PrintingContext context)
    {
        return Print(node, false, context);
    }

    public static Doc Print(RecursivePatternSyntax node, PrintingContext context)
    {
        return Print(node, true, context);
    }

    private static Doc Print(RecursivePatternSyntax node, bool includeType, PrintingContext context)
    {
        var result = new ValueListBuilder<Doc>([null, null, null, null, null, null, null]);
        if (node.Type != null && includeType)
        {
            result.Append(Node.Print(node.Type, context));
        }

        if (node.PositionalPatternClause != null)
        {
            result.Append(
                node.Parent
                    is SwitchExpressionArmSyntax
                        or IsPatternExpressionSyntax
                        or CasePatternSwitchLabelSyntax
                        or BinaryPatternSyntax
                        {
                            Parent: SwitchExpressionArmSyntax or CasePatternSwitchLabelSyntax
                        }
                    ? Doc.Null
                    : Doc.SoftLine,
                Doc.Group(
                    Token.Print(node.PositionalPatternClause.OpenParenToken, context),
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(
                            node.PositionalPatternClause.Subpatterns,
                            (subpatternNode, _) =>
                                Doc.Concat(
                                    subpatternNode.NameColon != null
                                        ? BaseExpressionColon.Print(
                                            subpatternNode.NameColon,
                                            context
                                        )
                                        : Doc.Null,
                                    Node.Print(subpatternNode.Pattern, context)
                                ),
                            Doc.Line,
                            context
                        )
                    ),
                    Doc.SoftLine,
                    Token.Print(node.PositionalPatternClause.CloseParenToken, context)
                )
            );
        }

        if (node.PropertyPatternClause != null)
        {
            if (!node.PropertyPatternClause.Subpatterns.Any())
            {
                if (node.Type != null)
                {
                    result.Append(" ");
                }
                result.Append("{ }");
            }
            else
            {
                result.Append(
                    Doc.Group(
                        node.Type != null
                        && !Enumerable.Any(
                            node.PropertyPatternClause.OpenBraceToken.LeadingTrivia,
                            o => o.IsDirective || o.IsComment()
                        )
                            ? Doc.Line
                            : Doc.Null,
                        Token.Print(node.PropertyPatternClause.OpenBraceToken, context),
                        Doc.Indent(
                            node.PropertyPatternClause.Subpatterns.Any() ? Doc.Line : Doc.Null,
                            SeparatedSyntaxList.Print(
                                node.PropertyPatternClause.Subpatterns,
                                (subpatternNode, _) =>
                                    Doc.Group(
                                        subpatternNode.ExpressionColon != null
                                            ? Node.Print(subpatternNode.ExpressionColon, context)
                                            : Doc.Null,
                                        Node.Print(subpatternNode.Pattern, context)
                                    ),
                                Doc.Line,
                                context
                            )
                        ),
                        Doc.Line,
                        Token.Print(node.PropertyPatternClause.CloseBraceToken, context)
                    )
                );
            }
        }

        if (node.Designation != null)
        {
            result.Append(" ", Node.Print(node.Designation, context));
        }

        return Doc.Concat(ref result);
    }
}
