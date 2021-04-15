using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(
            ConditionalExpressionSyntax node
        ) {
            return Docs.Group(
                Docs.Indent(
                    this.Print(node.Condition),
                    Docs.Line,
                    this.PrintSyntaxToken(
                        node.QuestionToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Docs.Indent(this.Print(node.WhenTrue)),
                    Docs.Line,
                    this.PrintSyntaxToken(
                        node.ColonToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Docs.Indent(this.Print(node.WhenFalse))
                )
            );
        }
    }
}
