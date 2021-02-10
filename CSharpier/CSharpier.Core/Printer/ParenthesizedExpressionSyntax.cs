using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintParenthesizedExpressionSyntax(ParenthesizedExpressionSyntax node)
        {
            return Concat("(", this.Print(node.Expression), ")");
        }
    }
}
