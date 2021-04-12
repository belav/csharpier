using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBracketedArgumentListSyntax(
            BracketedArgumentListSyntax node
        ) {
            return Docs.Group(
                this.PrintSyntaxToken(node.OpenBracketToken),
                Docs.Indent(
                    Docs.SoftLine,
                    this.PrintSeparatedSyntaxList(
                        node.Arguments,
                        this.Print,
                        Docs.Line
                    )
                ),
                Docs.SoftLine,
                this.PrintSyntaxToken(node.CloseBracketToken)
            );
        }
    }
}
