using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedLambdaExpressionSyntax(
            ParenthesizedLambdaExpressionSyntax node)
        {
            var parts = new Parts(
                this.PrintModifiers(node.Modifiers),
                this.PrintParameterListSyntax(node.ParameterList),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.ArrowToken, " "));
            if (node.ExpressionBody != null)
            {
                parts.Push(this.Print(node.ExpressionBody));
            }
            else if (node.Block != null)
            {
                parts.Push(this.PrintBlockSyntax(node.Block));
            }

            return Concat(parts);
        }
    }
}
