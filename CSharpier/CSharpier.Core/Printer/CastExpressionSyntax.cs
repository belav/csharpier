using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCastExpressionSyntax(CastExpressionSyntax node)
        {
            return Concat("(", this.Print(node.Type), ")", this.Print(node.Expression));
        }
    }
}
