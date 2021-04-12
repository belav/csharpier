using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPrimaryConstructorBaseTypeSyntax(
            PrimaryConstructorBaseTypeSyntax node
        ) {
            return Concat(
                this.Print(node.Type),
                this.PrintArgumentListSyntax(node.ArgumentList)
            );
        }
    }
}
