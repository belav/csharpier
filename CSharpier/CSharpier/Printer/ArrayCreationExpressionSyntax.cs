using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayCreationExpressionSyntax(ArrayCreationExpressionSyntax node)
        {
            return Group(
                Concat(
                    String(node.NewKeyword.Text),
                    String(" "),
                    this.Print(node.Type),
                    node.Initializer != null ? Concat(Line, this.Print(node.Initializer)) : String("")
                )
            );
        }
    }
}
