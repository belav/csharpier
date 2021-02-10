using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSimpleLambdaExpressionSyntax(SimpleLambdaExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Parameter),
                " ",
                node.ArrowToken.Text,
                " ",
                this.Print(node.Body)
            );
        }
    }
}
