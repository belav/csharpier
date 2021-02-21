using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTupleExpressionSyntax(TupleExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.OpenParenToken),
                this.PrintSeparatedSyntaxList(node.Arguments, this.Print, " "),
                this.PrintSyntaxToken(node.CloseParenToken));
        }
    }
}
