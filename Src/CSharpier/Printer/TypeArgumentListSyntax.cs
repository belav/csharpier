using System.Linq;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeArgumentListSyntax(TypeArgumentListSyntax node)
        {
            return Docs.Concat(
                SyntaxTokens.Print(node.LessThanToken),
                Docs.Indent(
                    this.PrintSeparatedSyntaxList(
                        node.Arguments,
                        this.Print,
                        Docs.Line
                    )
                ),
                SyntaxTokens.Print(node.GreaterThanToken)
            );
        }
    }
}
