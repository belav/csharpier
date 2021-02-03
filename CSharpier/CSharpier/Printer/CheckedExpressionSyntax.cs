using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCheckedExpressionSyntax(CheckedExpressionSyntax node)
        {
            return Concat(String(node.Keyword.Text), String("("), this.Print(node.Expression), String(")"));
        }
    }
}
