using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSimpleLambdaExpressionSyntax(SimpleLambdaExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Parameter),
                String(" "),
                String(node.ArrowToken.Text),
                String(" "),
                this.Print(node.Body)
            );
        }
    }
}
