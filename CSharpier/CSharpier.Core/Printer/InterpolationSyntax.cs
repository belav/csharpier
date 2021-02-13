using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInterpolationSyntax(InterpolationSyntax node)
        {
            var parts = new Parts(this.PrintSyntaxToken(node.OpenBraceToken),
                this.Print(node.Expression)
            );
            if (node.AlignmentClause != null) {
                parts.Push(this.PrintSyntaxToken(node.AlignmentClause.CommaToken, " "),
                    this.Print(node.AlignmentClause.Value));
            }
            if (node.FormatClause != null) {
                parts.Push(this.PrintSyntaxToken(node.FormatClause.ColonToken), this.PrintSyntaxToken(node.FormatClause.FormatStringToken));
            }

            parts.Push(this.PrintSyntaxToken(node.CloseBraceToken));
            return Concat(parts);
        }
    }
}
