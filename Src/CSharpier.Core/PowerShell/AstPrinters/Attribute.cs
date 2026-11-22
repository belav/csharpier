using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Attribute
{
    internal static Doc Print(AttributeAst node, PrintContext context)
    {
        var arguments = new List<Ast>(node.PositionalArguments.Count + node.NamedArguments.Count);
        arguments.AddRange(node.PositionalArguments);
        arguments.AddRange(node.NamedArguments);
        arguments.Sort(
            (first, second) => first.Extent.StartOffset.CompareTo(second.Extent.StartOffset)
        );

        var tokens = context.GetTokens(node.Extent.StartOffset, node.Extent.EndOffset);
        var openParenthesis = tokens.Find(o => o.Kind == TokenKind.LParen);
        var closeParenthesis = tokens.FindLast(o => o.Kind == TokenKind.RParen);

        return Doc.Concat(
            "[",
            node.TypeName.Extent.Text,
            ArgumentList.Print(
                "(",
                arguments,
                ")",
                openParenthesis?.Extent.EndOffset ?? node.Extent.StartOffset,
                closeParenthesis?.Extent.StartOffset ?? node.Extent.EndOffset,
                context
            ),
            "]"
        );
    }
}
