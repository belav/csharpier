using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class UnaryExpression
{
    public static Doc Print(UnaryExpressionAst node, PrintContext context)
    {
        var child = Node.Print(node.Child, context);

        if (node.TokenKind is TokenKind.PostfixPlusPlus or TokenKind.PostfixMinusMinus)
        {
            return Doc.Concat(
                child,
                context.GetTokenText(node.Child.Extent.EndOffset, node.Extent.EndOffset)
            );
        }

        var operatorTokens = context.GetTokens(
            node.Extent.StartOffset,
            node.Child.Extent.StartOffset
        );
        var unaryOperator = string.Concat(operatorTokens.Select(o => o.Text));
        if (unaryOperator.Length > 1 && char.IsLetter(unaryOperator[1]))
        {
            return Doc.Concat(unaryOperator.ToLowerInvariant(), " ", child);
        }

        var keepSpace =
            node.TokenKind is TokenKind.Minus or TokenKind.Plus
            && operatorTokens.Count > 0
            && operatorTokens[^1].Extent.EndOffset != node.Child.Extent.StartOffset;

        return Doc.Concat(unaryOperator, keepSpace ? " " : Doc.Null, child);
    }
}
