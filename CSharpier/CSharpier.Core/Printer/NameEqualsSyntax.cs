using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintNameEqualsSyntax(NameEqualsSyntax node)
        {
            return Concat(this.Print(node.Name), " = ");
        }
    }
}
