using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintObjectCreationExpressionSyntax(ObjectCreationExpressionSyntax node)
        {
            return Concat(
                String("new"),
                String(" "),
                this.Print(node.Type),
                node.ArgumentList != null ? this.Print(node.ArgumentList) : "",
                node.Initializer != null ? this.Print(node.Initializer) : ""
            );
        }
    }
}
