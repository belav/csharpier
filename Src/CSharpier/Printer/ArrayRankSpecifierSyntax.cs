using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayRankSpecifierSyntax(ArrayRankSpecifierSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.OpenBracketToken),
                node.Sizes.Any()
                    ? this.PrintSeparatedSyntaxList(
                        node.Sizes,
                        this.Print,
                        null
                    )
                    : null,
                this.PrintSyntaxToken(node.CloseBracketToken)
            );
        }
    }
}
