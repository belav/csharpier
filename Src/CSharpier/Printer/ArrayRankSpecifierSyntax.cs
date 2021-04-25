using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayRankSpecifierSyntax(
            ArrayRankSpecifierSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.OpenBracketToken),
                node.Sizes.Any()
                    ? SeparatedSyntaxList.Print(
                            node.Sizes,
                            this.Print,
                            Doc.Null
                        )
                    : Doc.Null,
                SyntaxTokens.Print(node.CloseBracketToken)
            );
        }
    }
}
