using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            return String(node.Identifier.Text);
        }
    }
}
