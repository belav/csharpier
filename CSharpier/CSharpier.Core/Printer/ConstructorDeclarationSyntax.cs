using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(ConstructorDeclarationSyntax node)
        {
            return Group(Concat(this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                this.PrintSyntaxToken(node.Identifier),
                this.PrintParameterListSyntax(node.ParameterList),
                node.Initializer != null ? this.Print(node.Initializer) : null,
                this.PrintBlockSyntax(node.Body)));
        }
    }
}