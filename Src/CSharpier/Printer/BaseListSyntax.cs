using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                        SeparatedSyntaxList.Print(
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
