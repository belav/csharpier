using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintParenthesizedLambdaExpressionSyntax(ParenthesizedLambdaExpressionSyntax node)
        {
            var parts = new Parts();
            if (node.AsyncKeyword.RawKind != 0) {
                parts.Push("async ");
            }
            parts.Push(
                this.PrintParameterListSyntax(node.ParameterList),
                " => "
            );
            if (node.ExpressionBody != null) {
                parts.Push(this.Print(node.ExpressionBody));
            } else if (node.Block != null) {
                parts.Push(this.PrintBlockSyntax(node.Block));
            }
            return Concat(parts);
        }
    }
}
