using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentListSyntax(ArgumentListSyntax node)
        {
            if (node.Arguments.Count == 0)
            {
                return String("()");
            }

            return Group(Concat(String("("), Indent(Concat(SoftLine, this.PrintCommaList(node.Arguments.Select(this.Print)))), String(")")));
        }
    }
}