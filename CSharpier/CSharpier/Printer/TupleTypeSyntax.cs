using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleTypeSyntax(TupleTypeSyntax node)
        {
            return Concat(String("("), Join(String(", "), node.Elements.Select(this.Print)), String(")"));
        }
    }
}
