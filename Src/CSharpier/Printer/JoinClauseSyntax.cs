using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.JoinKeyword, " "),
                this.PrintSyntaxToken(node.Identifier, " "),
                this.PrintSyntaxToken(node.InKeyword, " "),
                this.Print(node.InExpression),
                Indent(
                    Line,
                    this.PrintSyntaxToken(node.OnKeyword, " "),
                    this.Print(node.LeftExpression),
                    SpaceIfNoPreviousComment,
                    this.PrintSyntaxToken(node.EqualsKeyword, " "),
                    this.Print(node.RightExpression),
                    node.Into != null
                        ? Concat(
                            Line,
                            this.PrintSyntaxToken(node.Into.IntoKeyword, " "),
                            this.PrintSyntaxToken(node.Into.Identifier)
                        )
                        : Doc.Null
                )
            );
        }
    }
}
