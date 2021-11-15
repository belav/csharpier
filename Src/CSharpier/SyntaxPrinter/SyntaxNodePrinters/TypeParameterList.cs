using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeParameterList
{
    public static Doc Print(TypeParameterListSyntax node)
    {
        if (node.Parameters.Count == 0)
        {
            return Doc.Null;
        }

        var shouldBreakMore =
            node.Parameters.Count > 1 || node.Parameters.First().AttributeLists.Any();

        return Doc.Group(
            Token.Print(node.LessThanToken),
            Doc.Indent(
                shouldBreakMore ? Doc.SoftLine : Doc.Null,
                SeparatedSyntaxList.Print(node.Parameters, TypeParameter.Print, Doc.Line)
            ),
            shouldBreakMore ? Doc.SoftLine : Doc.Null,
            Token.Print(node.GreaterThanToken)
        );
    }
}
