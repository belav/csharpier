using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefExpressionSyntax(RefExpressionSyntax node)
        {
            return Concat(String(node.RefKeyword.Text), String(" "), this.Print(node.Expression));
        }
    }
}
