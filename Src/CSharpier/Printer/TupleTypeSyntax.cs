using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleTypeSyntax(TupleTypeSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                SeparatedSyntaxList.Print(node.Elements, this.Print, " "),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
