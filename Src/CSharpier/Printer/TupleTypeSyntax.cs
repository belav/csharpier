using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleTypeSyntax(TupleTypeSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(node.OpenParenToken),
                this.PrintSeparatedSyntaxList(node.Elements, this.Print, " "),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
