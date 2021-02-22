using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintObjectCreationExpressionSyntax(
            ObjectCreationExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.NewKeyword, " "),
                this.Print(node.Type),
                node.ArgumentList != null ? this.Print(node.ArgumentList) : "",
                node.Initializer != null ? this.Print(node.Initializer) : "");
        }
    }
}
