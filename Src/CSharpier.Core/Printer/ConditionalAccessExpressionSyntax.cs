using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConditionalAccessExpressionSyntax(ConditionalAccessExpressionSyntax node)
        {
            return Concat(this.Print(node.Expression), this.PrintSyntaxToken(node.OperatorToken), this.Print(node.WhenNotNull));
        }
    }
}
