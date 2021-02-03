using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIsPatternExpressionSyntax(IsPatternExpressionSyntax node)
        {
            return Concat(this.Print(node.Expression), String(" is "), this.Print(node.Pattern));
        }
    }
}
