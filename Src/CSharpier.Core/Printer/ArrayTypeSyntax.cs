using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintArrayTypeSyntax(ArrayTypeSyntax node)
        {
            return Concat(this.Print(node.ElementType), Concat(node.RankSpecifiers.Select(this.Print).ToArray()));
        }
    }
}
