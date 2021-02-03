using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayRankSpecifierSyntax(ArrayRankSpecifierSyntax node)
        {
            return Concat(String("["), Join(String(","), node.Sizes.Select(this.Print)), String("]"));
        }
    }
}
