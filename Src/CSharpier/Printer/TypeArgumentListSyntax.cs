using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeArgumentListSyntax(TypeArgumentListSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(node.LessThanToken),
                Docs.Indent(
                    this.PrintSeparatedSyntaxList(
                        node.Arguments,
                        this.Print,
                        Docs.Line
                    )
                ),
                this.PrintSyntaxToken(node.GreaterThanToken)
            );
        }
    }
}
