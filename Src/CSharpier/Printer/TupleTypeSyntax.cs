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
            return Docs.Concat(
                SyntaxTokens.Print(node.OpenParenToken),
                SeparatedSyntaxList.Print(node.Elements, this.Print, " "),
                SyntaxTokens.Print(node.CloseParenToken)
            );
        }
    }
}
