using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ArrayRankSpecifier
    {
        public static Doc Print(ArrayRankSpecifierSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenBracketToken),
                node.Sizes.Any()
                    ? Doc.Concat(
                            Doc.Indent(
                                Doc.SoftLine,
                                SeparatedSyntaxList.Print(node.Sizes, Node.Print, Doc.Null)
                            ),
                            Doc.SoftLine
                        )
                    : Doc.Null,
                Token.Print(node.CloseBracketToken)
            );
        }
    }
}

