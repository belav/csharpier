using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhenClause
{
    public static Doc Print(WhenClauseSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Doc.Indent(
                node.Parent is SwitchExpressionArmSyntax { Pattern: DiscardPatternSyntax }
                    ? " "
                    : Doc.Line,
                Token.PrintWithSuffix(node.WhenKeyword, " ", context),
                Node.Print(node.Condition, context)
            )
        );
    }
}
