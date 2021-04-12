using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseListSyntax(BaseListSyntax node)
        {
            return Docs.Group(
                Docs.Indent(
                    Docs.Line,
                    this.PrintSyntaxToken(
                        node.ColonToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Docs.Indent(
                        this.PrintSeparatedSyntaxList(
                            node.Types,
                            this.Print,
                            Docs.Line
                        )
                    )
                )
            );
        }
    }
}
