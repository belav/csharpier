using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConditionalExpressionSyntax(
            ConditionalExpressionSyntax node
        ) {
            return Doc.Group(
                Doc.Indent(
                    this.Print(node.Condition),
                    Doc.Line,
                    this.PrintSyntaxToken(
                        node.QuestionToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Doc.Indent(this.Print(node.WhenTrue)),
                    Doc.Line,
                    this.PrintSyntaxToken(
                        node.ColonToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    Doc.Indent(this.Print(node.WhenFalse))
                )
            );
        }
    }
}
