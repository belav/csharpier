using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(ConditionalExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Condition),
                String(" ? "),
                this.Print(node.WhenTrue),
                String(" : "),
                this.Print(node.WhenFalse)
            );
        }
    }
}
