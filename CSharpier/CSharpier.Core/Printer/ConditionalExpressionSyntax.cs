using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(ConditionalExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Condition),
                " ? ",
                this.Print(node.WhenTrue),
                " : ",
                this.Print(node.WhenFalse)
            );
        }
    }
}
