using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInterpolationSyntax(InterpolationSyntax node)
        {
            var docs = new List<Doc>
            {
                Token.Print(node.OpenBraceToken),
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
                    Token.Print(node.FormatClause.ColonToken),
                    Token.Print(node.FormatClause.FormatStringToken)
                );
            }

            docs.Add(Token.Print(node.CloseBraceToken));
            return Doc.Concat(docs);
        }
    }
}
