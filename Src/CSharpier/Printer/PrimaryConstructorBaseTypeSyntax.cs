using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPrimaryConstructorBaseTypeSyntax(
            PrimaryConstructorBaseTypeSyntax node
        ) {
            return Docs.Concat(
                this.Print(node.Type),
                this.PrintArgumentListSyntax(node.ArgumentList)
            );
        }
    }
}
