using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Doc.Group(
                this.PrintSyntaxToken(
                    node.JoinKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.Identifier,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.InKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.InExpression),
                Doc.Indent(
                    Doc.Line,
                    this.PrintSyntaxToken(
                        node.OnKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.Print(node.LeftExpression),
                    " ",
                    this.PrintSyntaxToken(
                        node.EqualsKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.Print(node.RightExpression),
                    node.Into != null
                        ? Doc.Concat(
                                Doc.Line,
                                this.PrintSyntaxToken(
                                    node.Into.IntoKeyword,
                                    afterTokenIfNoTrailing: " "
                                ),
                                Token.Print(node.Into.Identifier)
                            )
                        : Doc.Null
                )
            );
        }
    }
}
