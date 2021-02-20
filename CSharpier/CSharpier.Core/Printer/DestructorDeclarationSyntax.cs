using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDestructorDeclarationSyntax(DestructorDeclarationSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintSyntaxToken(node.TildeToken),
                this.PrintSyntaxToken(node.Identifier),
                this.Print(node.ParameterList),
                this.Print(node.Body),
                this.Print(node.ExpressionBody),
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}