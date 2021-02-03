using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCastExpressionSyntax(CastExpressionSyntax node)
        {
            return Concat(String("("), this.Print(node.Type), String(")"), this.Print(node.Expression));
        }
    }
}
