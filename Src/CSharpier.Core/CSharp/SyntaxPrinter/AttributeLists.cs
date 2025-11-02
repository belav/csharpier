using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class AttributeLists
{
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

        var docs = new ValueListBuilder<Doc>([null, null]);
        Doc separator = node
            is TypeParameterSyntax
                or ParameterSyntax
                or ParenthesizedLambdaExpressionSyntax
                or AccessorDeclarationSyntax
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
