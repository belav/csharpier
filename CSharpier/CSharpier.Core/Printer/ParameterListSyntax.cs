using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintParameterListSyntax(ParameterListSyntax node)
        {
            if (node.Parameters.Count == 0) {
                return "()";
            }
            return Group(Concat("(", Indent(Concat(SoftLine, this.PrintCommaList(node.Parameters.Select(this.Print)))), ")"));
        }
    }
}
