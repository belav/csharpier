using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPropertyDeclarationSyntax(PropertyDeclarationSyntax node)
        {
            return this.PrintBasePropertyDeclarationSyntax(node);
        }
    }
}
