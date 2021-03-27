using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(
            ConditionalExpressionSyntax node)
        {
            return Group(
                Indent(
                    this.Print(node.Condition),
                    Line,
                    this.PrintSyntaxToken(node.QuestionToken, " "),
                    this.Print(node.WhenTrue),
                    Line,
                    this.PrintSyntaxToken(node.ColonToken, " "),
                    this.Print(node.WhenFalse)
                )
            );
        }
    }
}
