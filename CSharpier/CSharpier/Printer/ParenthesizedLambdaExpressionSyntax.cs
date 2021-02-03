using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedLambdaExpressionSyntax(ParenthesizedLambdaExpressionSyntax node)
        {
            var parts = new Parts();
            if (NotNull(node.AsyncKeyword)) {
                parts.Push(String("async "));
            }
            parts.Push(
                this.PrintParameterListSyntax(node.ParameterList),
                String(" => ")
            );
            if (NotNull(node.ExpressionBody)) {
                parts.Push(this.Print(node.ExpressionBody));
            } else if (NotNull(node.Block)) {
                parts.Push(this.PrintBlockSyntax(node.Block));
            }
            return Concat(parts);
        }
    }
}
