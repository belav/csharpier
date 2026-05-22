using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class AttributeLists
{
    internal static int GetTotalAttributeCount(SyntaxList<AttributeListSyntax> attributeLists) =>
        attributeLists.Sum(list => list.Attributes.Count);

    internal static bool HasOriginalLineBreaks(SyntaxList<AttributeListSyntax> attributeLists)
    {
        for (var i = 0; i < attributeLists.Count - 1; i++)
        {
            if (attributeLists[i].GetTrailingTrivia().Any(SyntaxKind.EndOfLineTrivia))
            {
                return true;
            }
        }
        return false;
    }

    public static Doc Print(
        SyntaxNode node,
        SyntaxList<AttributeListSyntax> attributeLists,
        PrintingContext context
    )
    {
        if (attributeLists.Count == 0)
        {
            return Doc.Null;
        }

        var isFieldSameLine =
            node is BaseFieldDeclarationSyntax
            && context.Options.AllowFieldAttributeOnSameLine;

        if (isFieldSameLine)
        {
            var totalAttrs = GetTotalAttributeCount(attributeLists);
            if (totalAttrs == 3 && !HasOriginalLineBreaks(attributeLists))
            {
                return Doc.Concat(
                    Doc.Group(
                        Doc.Join(
                            Doc.Line,
                            attributeLists.Select(o => AttributeList.Print(o, context))
                        )
                    ),
                    Doc.HardLine
                );
            }

            if (
                totalAttrs <= 2
                && attributeLists.Count > 1
                && HasOriginalLineBreaks(attributeLists)
            )
            {
                var result = new DocListBuilder(attributeLists.Count * 2);
                for (var i = 0; i < attributeLists.Count; i++)
                {
                    result.Add(AttributeList.Print(attributeLists[i], context));
                    if (i < attributeLists.Count - 1)
                    {
                        result.Add(
                            attributeLists[i]
                                .GetTrailingTrivia()
                                .Any(SyntaxKind.EndOfLineTrivia)
                                ? Doc.HardLine
                                : (Doc)" "
                        );
                    }
                }
                result.Add(" ");
                return Doc.Concat(ref result);
            }
        }

        var docs = new DocListBuilder(2);
        Doc separator = node
            is TypeParameterSyntax
                or ParameterSyntax
                or ParenthesizedLambdaExpressionSyntax
                or AccessorDeclarationSyntax
            ? Doc.Line
            : isFieldSameLine && GetTotalAttributeCount(attributeLists) <= 2
                ? Doc.Line
                : Doc.HardLine;

        docs.Add(Doc.Join(separator, attributeLists.Select(o => AttributeList.Print(o, context))));

        if (node is not (ParameterSyntax or TypeParameterSyntax))
        {
            docs.Add(separator);
        }

        return Doc.Concat(ref docs);
    }
}
