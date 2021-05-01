using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeArgumentListSyntax(TypeArgumentListSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.LessThanToken),
                Doc.Indent(
                    SeparatedSyntaxList.Print(
                        node.Arguments,
                        this.Print,
                        Doc.Line
                    )
                ),
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
