using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintObjectCreationExpressionSyntax(ObjectCreationExpressionSyntax node)
        {
            return Concat(
                "new",
                " ",
                this.Print(node.Type),
                node.ArgumentList != null ? this.Print(node.ArgumentList) : "",
                node.Initializer != null ? this.Print(node.Initializer) : ""
            );
        }
    }
}
