using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBracketedArgumentListSyntax(
            BracketedArgumentListSyntax node
        ) {
            return Docs.Group(
                SyntaxTokens.Print(node.OpenBracketToken),
                Docs.Indent(
                    Docs.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.Arguments,
                        this.Print,
                        Docs.Line
                    )
                ),
                Docs.SoftLine,
                SyntaxTokens.Print(node.CloseBracketToken)
            );
        }
    }
}
