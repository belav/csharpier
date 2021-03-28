using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintObjectCreationExpressionSyntax(
            ObjectCreationExpressionSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.NewKeyword, " "),
                this.Print(node.Type),
                node.ArgumentList != null
                    ? this.PrintArgumentListSyntax(node.ArgumentList)
                    : string.Empty,
                node.Initializer != null
                    ? this.PrintInitializerExpressionSyntax(node.Initializer)
                    : string.Empty
            );
        }
    }
}
