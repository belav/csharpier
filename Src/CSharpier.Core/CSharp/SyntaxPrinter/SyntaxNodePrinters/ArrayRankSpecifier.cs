using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayRankSpecifier
{
    public static Doc Print(ArrayRankSpecifierSyntax node, PrintingContext context)
    {
        if (node.Sizes.All(o => o is OmittedArraySizeExpressionSyntax))
        {
            return Doc.Concat(
                Token.Print(node.OpenBracketToken, context),
                SeparatedSyntaxList.Print(node.Sizes, Node.Print, Doc.Null, context),
                Token.Print(node.CloseBracketToken, context)
            );
        }

        return Doc.Group(
            Token.Print(node.OpenBracketToken, context),
            node.Sizes.Any()
                ? Doc.Concat(
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(node.Sizes, Node.Print, Doc.Line, context)
                    ),
                    Doc.SoftLine
                )
                : Doc.Null,
            Token.Print(node.CloseBracketToken, context)
        );
    }
}
