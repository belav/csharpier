using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolationSyntax(InterpolationSyntax node)
        {
            var docs = new List<Doc>
            {
                this.PrintSyntaxToken(node.OpenBraceToken),
                this.Print(node.Expression)
            };
            if (node.AlignmentClause != null)
            {
                docs.Add(
                    this.PrintSyntaxToken(
                        node.AlignmentClause.CommaToken,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.Print(node.AlignmentClause.Value)
                );
            }
            if (node.FormatClause != null)
            {
                docs.Add(
                    this.PrintSyntaxToken(node.FormatClause.ColonToken),
                    this.PrintSyntaxToken(node.FormatClause.FormatStringToken)
                );
            }

            docs.Add(this.PrintSyntaxToken(node.CloseBraceToken));
            return Docs.Concat(docs);
        }
    }
}
