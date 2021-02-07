using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousMethodExpressionSyntax(AnonymousMethodExpressionSyntax node)
        {
            var parts = new Parts();
            if (node.AsyncKeyword.RawKind != 0) {
                parts.Add(String("async "));
            }
            if (node.DelegateKeyword.RawKind != 0) {
                parts.Add(String("delegate"));
            }
            if (node.ParameterList != null) {
                parts.Add(this.PrintParameterListSyntax(node.ParameterList));
            }
            if (node.ExpressionBody != null) {
                parts.Add(this.Print(node.ExpressionBody));
                // TODO why is this never null?
            } else if (node.Block != null) {
                parts.Add(this.PrintBlockSyntax(node.Block));
            }
            return Concat(parts);
        }
    }
}
