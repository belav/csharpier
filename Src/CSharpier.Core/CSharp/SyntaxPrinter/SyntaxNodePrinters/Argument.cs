using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class Argument
{
    public static Doc Print(ArgumentSyntax node, CSharpPrintingContext context)
    {
        var modifiers = PrintModifiers(node, context);

        return modifiers == Doc.Null
            ? Node.Print(node.Expression, context)
            : Doc.Concat(modifiers, Node.Print(node.Expression, context));
    }

    public static Doc PrintModifiers(ArgumentSyntax node, CSharpPrintingContext context)
    {
        var hasRefKind = node.RefKindKeyword.RawSyntaxKind() != SyntaxKind.None;

        if (node.NameColon == null)
        {
            return hasRefKind ? Token.PrintWithSuffix(node.RefKindKeyword, " ", context) : Doc.Null;
        }

        var nameColon = BaseExpressionColon.Print(node.NameColon, context);

        return hasRefKind
            ? Doc.Concat(nameColon, Token.PrintWithSuffix(node.RefKindKeyword, " ", context))
            : nameColon;
    }
}
