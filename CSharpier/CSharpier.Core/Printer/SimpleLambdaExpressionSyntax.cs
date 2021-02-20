using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSimpleLambdaExpressionSyntax(SimpleLambdaExpressionSyntax node)
        {
            return Concat(
                this.PrintModifiers(node.Modifiers),
                this.Print(node.Parameter),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.ArrowToken, " "),
                this.Print(node.Body)
            );
        }
    }
}
