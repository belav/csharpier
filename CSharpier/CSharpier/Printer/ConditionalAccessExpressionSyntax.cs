using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalAccessExpressionSyntax(ConditionalAccessExpressionSyntax node)
        {
            return Concat(this.Print(node.Expression), String("?"), this.Print(node.WhenNotNull));
        }
    }
}
