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
            return Doc.Group(
                Doc.Indent(
                    Doc.Line,
                    this.PrintSyntaxToken(
                        node.ColonToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Doc.Indent(
                        SeparatedSyntaxList.Print(
                            node.Types,
                            this.Print,
                            Doc.Line
                        )
                    )
                )
            );
        }
    }
}
