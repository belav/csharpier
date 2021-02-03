using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNameColonSyntax(NameColonSyntax node)
        {
            return Concat(node.Name.Identifier.Text, String(": "));
        }
    }
}
