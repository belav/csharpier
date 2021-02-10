using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTupleTypeSyntax(TupleTypeSyntax node)
        {
            return Concat("(", Join(", ", node.Elements.Select(this.Print)), ")");
        }
    }
}
