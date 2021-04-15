using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDeclarationExpressionSyntax(
            DeclarationExpressionSyntax node
        ) {
            return Docs.Concat(
                this.Print(node.Type),
                " ",
                this.Print(node.Designation)
            );
        }
    }
}
