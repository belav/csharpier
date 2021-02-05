using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousMethodExpressionSyntax(AnonymousMethodExpressionSyntax node)
        {
            var parts = new Parts();
            if (NotNullToken(node.AsyncKeyword)) {
                parts.Add(String("async "));
            }
            if (NotNullToken(node.DelegateKeyword)) {
                parts.Add(String("delegate"));
            }
            if (node.ParameterList != null) {
                parts.Add(this.PrintParameterListSyntax(node.ParameterList));
            }
            if (node.ExpressionBody != null) {
                parts.Add(this.Print(node.ExpressionBody));
            } else if (node.Block != null) {
                parts.Add(this.PrintBlockSyntax(node.Block));
            }
            return Concat(parts);
        }
    }
}
