using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentListSyntax(ArgumentListSyntax node)
        {
            return Group(
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
        }
    }
}
