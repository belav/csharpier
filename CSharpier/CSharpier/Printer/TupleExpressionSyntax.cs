using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleExpressionSyntax(TupleExpressionSyntax node)
        {
            return Concat(String("("), Join(String(", "), node.Arguments.Select(this.Print)), String(")"));
        }
    }
}
