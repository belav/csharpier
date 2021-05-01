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
            return Doc.Group(
                Token.Print(node.OpenBracketToken),
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.Arguments,
                        this.Print,
                        Doc.Line
                    )
                ),
                Doc.SoftLine,
                Token.Print(node.CloseBracketToken)
            );
        }
    }
}
