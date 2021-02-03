using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedExpressionSyntax(ParenthesizedExpressionSyntax node)
        {
            return Concat(String("("), this.Print(node.Expression), String(")"));
        }
    }
}
