using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPointerTypeSyntax(PointerTypeSyntax node)
        {
            return Concat(this.Print(node.ElementType), String(node.AsteriskToken.Text));
        }
    }
}
