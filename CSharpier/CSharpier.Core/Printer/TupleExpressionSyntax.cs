using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTupleExpressionSyntax(TupleExpressionSyntax node)
        {
            return Concat("(", Join(", ", node.Arguments.Select(this.Print)), ")");
        }
    }
}
