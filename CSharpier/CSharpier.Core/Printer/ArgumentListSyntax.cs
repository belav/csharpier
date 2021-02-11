using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 0 trivia start here!
        private Doc PrintArgumentListSyntax(ArgumentListSyntax node)
        {
            if (node.Arguments.Count == 0)
            {
                return "()";
            }

            return Group(Concat("(", Indent(Concat(SoftLine, this.PrintCommaList(node.Arguments.Select(this.Print)))), ")"));
        }
    }
}