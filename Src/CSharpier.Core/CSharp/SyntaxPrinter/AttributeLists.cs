using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class AttributeLists
{
    public static Doc Print(
        SyntaxNode node,
        SyntaxList<AttributeListSyntax> attributeLists,
        CSharpPrintingContext context
    )
    {
        if (attributeLists.Count == 0)
        {
            return Doc.Null;
        }

        Doc separator = node
            is TypeParameterSyntax
                or ParameterSyntax
                or ParenthesizedLambdaExpressionSyntax
                or AccessorDeclarationSyntax
            ? Doc.Line
            : Doc.HardLine;

        var printedLists = Doc.Join(separator, attributeLists, AttributeList.Print, context);

        return node is ParameterSyntax or TypeParameterSyntax
            ? printedLists
            : Doc.Concat(printedLists, separator);
    }
}
