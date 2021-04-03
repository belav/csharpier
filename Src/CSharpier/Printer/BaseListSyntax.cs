using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseListSyntax(BaseListSyntax node)
        {
            return Group(
                Indent(
                    Line,
                    this.PrintSyntaxToken(
                        node.ColonToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Indent(
                        this.PrintSeparatedSyntaxList(
                            node.Types,
                            this.Print,
                            Line
                        )
                    )
                )
            );
        }
    }
}
