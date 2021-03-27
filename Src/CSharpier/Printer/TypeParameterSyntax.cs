using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterSyntax(TypeParameterSyntax node)
        {
            return Concat(
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintSyntaxToken(node.VarianceKeyword, " "),
                this.PrintSyntaxToken(node.Identifier)
            );
        }
    }
}
