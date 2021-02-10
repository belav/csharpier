using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintNameColonSyntax(NameColonSyntax node)
        {
            return Concat(node.Name.Identifier.Text, ": ");
        }
    }
}
