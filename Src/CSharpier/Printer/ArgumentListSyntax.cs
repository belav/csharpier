using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentListSyntax(ArgumentListSyntax node)
        {
            var result = Concat(
                this.PrintSyntaxToken(node.OpenParenToken),
                node.Arguments.Any()
                    ? Indent(
                        SoftLine,
                        this.PrintSeparatedSyntaxList(
                            node.Arguments,
                            this.PrintArgumentSyntax,
                            Line))
                    : null,
                this.PrintSyntaxToken(node.CloseParenToken));

            return node.Parent is not ObjectCreationExpressionSyntax
                ? Group(result)
                : result;
        }
    }
}
