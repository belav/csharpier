using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDeclarationExpressionSyntax(DeclarationExpressionSyntax node)
        {
            return Concat(this.Print(node.Type), String(" "), this.Print(node.Designation));
        }
    }
}
