using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Group(
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
                Indent(
                    Line,
                    this.PrintSyntaxToken(
                        node.OnKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.Print(node.LeftExpression),
                    SpaceIfNoPreviousComment,
                    this.PrintSyntaxToken(
                        node.EqualsKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.Print(node.RightExpression),
                    node.Into != null
                        ? Concat(
                                Line,
                                this.PrintSyntaxToken(
                                    node.Into.IntoKeyword,
                                    afterTokenIfNoTrailing: " "
                                ),
                                this.PrintSyntaxToken(node.Into.Identifier)
                            )
                        : Doc.Null
                )
            );
        }
    }
}
