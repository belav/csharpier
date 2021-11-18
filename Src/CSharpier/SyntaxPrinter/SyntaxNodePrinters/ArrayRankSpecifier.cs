using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayRankSpecifier
{
    public static Doc Print(ArrayRankSpecifierSyntax node)
    {
        if (node.Sizes.All(o => o is OmittedArraySizeExpressionSyntax))
        {
            return Doc.Concat(
                Token.Print(node.OpenBracketToken),
                SeparatedSyntaxList.Print(node.Sizes, Node.Print, Doc.Null),
                Token.Print(node.CloseBracketToken)
            );
        }

        return Doc.Group(
            Token.Print(node.OpenBracketToken),
            node.Sizes.Any()
              ? Doc.Concat(
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(node.Sizes, Node.Print, Doc.Line)
                    ),
                    Doc.SoftLine
                )
              : Doc.Null,
            Token.Print(node.CloseBracketToken)
        );
    }
}
