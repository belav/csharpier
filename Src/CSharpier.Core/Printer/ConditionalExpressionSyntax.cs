using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(ConditionalExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Condition),
                " ",
                this.PrintSyntaxToken(node.QuestionToken, " "),
                this.Print(node.WhenTrue),
                " ",
                this.PrintSyntaxToken(node.ColonToken, " "),
                this.Print(node.WhenFalse)
            );
        }
    }
}
