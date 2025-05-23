using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeParameterList
{
    public static Doc Print(TypeParameterListSyntax node, PrintingContext context)
    {
        if (node.Parameters.Count == 0)
        {
            return Doc.Null;
        }

        var shouldBreakMore =
            node.Parameters.Count > 1 || node.Parameters.First().AttributeLists.Any();

        return Doc.Group(
            Token.Print(node.LessThanToken, context),
            Doc.Indent(
                shouldBreakMore ? Doc.SoftLine : Doc.Null,
                SeparatedSyntaxList.Print(node.Parameters, TypeParameter.Print, Doc.Line, context)
            ),
            shouldBreakMore ? Doc.SoftLine : Doc.Null,
            Token.Print(node.GreaterThanToken, context)
        );
    }
}
