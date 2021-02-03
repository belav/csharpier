using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterListSyntax(TypeParameterListSyntax node)
        {
            if (node.Parameters.Count == 0) {
                return "";
            }
            return Group(Concat("<", Indent(Concat(SoftLine, this.PrintCommaList(node.Parameters.Select(this.Print)))), ">"));
        }
    }
}
