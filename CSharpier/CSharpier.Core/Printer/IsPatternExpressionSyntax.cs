using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintIsPatternExpressionSyntax(IsPatternExpressionSyntax node)
        {
            return Concat(this.Print(node.Expression), " is ", this.Print(node.Pattern));
        }
    }
}
