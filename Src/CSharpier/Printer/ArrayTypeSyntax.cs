using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayTypeSyntax(ArrayTypeSyntax node)
        {
            return Docs.Concat(
                this.Print(node.ElementType),
                Docs.Concat(node.RankSpecifiers.Select(this.Print).ToArray())
            );
        }
    }
}
