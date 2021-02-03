using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousMethodExpressionSyntax(AnonymousMethodExpressionSyntax node)
        {
            var parts = new Parts();
            if (NotNull(node.AsyncKeyword)) {
                parts.Push(String("async "));
            }
            if (NotNull(node.DelegateKeyword)) {
                parts.Push(String("delegate"));
            }
            if (NotNull(node.ParameterList)) {
                parts.Push(this.PrintParameterListSyntax(node.ParameterList));
            }
            if (NotNull(node.ExpressionBody)) {
                parts.Push(this.Print(node.ExpressionBody));
            } else if (NotNull(node.Block)) {
                parts.Push(this.PrintBlockSyntax(node.Block));
            }
            return Concat(parts);
        }
    }
}
