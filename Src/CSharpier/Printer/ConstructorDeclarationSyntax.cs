using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(
            ConstructorDeclarationSyntax node)
        {
            return Group(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                this.PrintSyntaxToken(node.Identifier),
                this.PrintParameterListSyntax(node.ParameterList),
                node.Initializer != null
                    ? this.Print(node.Initializer)
                    : Doc.Null,
                node.Body != null ? this.PrintBlockSyntax(node.Body) : Doc.Null,
                node.ExpressionBody != null
                    ? this.PrintArrowExpressionClauseSyntax(node.ExpressionBody)
                    : Doc.Null,
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
