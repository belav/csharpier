using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeArgumentListSyntax(TypeArgumentListSyntax node)
        {
            return Indent(Group(this.PrintCommaList(node.Arguments.Select(this.Print))));
        }
    }
}
