using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeArgumentListSyntax(TypeArgumentListSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.LessThanToken),
                Indent(
                    this.PrintSeparatedSyntaxList(
                        node.Arguments,
                        this.Print,
                        Line)),
                this.PrintSyntaxToken(node.GreaterThanToken));
        }
    }
}
