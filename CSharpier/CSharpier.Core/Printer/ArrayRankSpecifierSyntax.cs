using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintArrayRankSpecifierSyntax(ArrayRankSpecifierSyntax node)
        {
            return Concat("[", Join(",", node.Sizes.Select(this.Print)), "]");
        }
    }
}
