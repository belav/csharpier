using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAwaitExpressionSyntax(AwaitExpressionSyntax node)
        {
            return Concat(String(node.AwaitKeyword.Text), String(" "), this.Print(node.Expression));
        }
    }
}
