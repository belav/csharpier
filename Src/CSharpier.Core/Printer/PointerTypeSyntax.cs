using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPointerTypeSyntax(PointerTypeSyntax node)
        {
            return Concat(this.Print(node.ElementType), this.PrintSyntaxToken(node.AsteriskToken));
        }
    }
}
