using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayTypeSyntax(ArrayTypeSyntax node)
        {
            return Doc.Concat(
                this.Print(node.ElementType),
                Doc.Concat(node.RankSpecifiers.Select(this.Print).ToArray())
            );
        }
    }
}
