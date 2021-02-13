using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.JoinKeyword, " "),
                this.PrintSyntaxToken(node.Identifier, " "),
                this.PrintSyntaxToken(node.InKeyword, " "),
                this.Print(node.InExpression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.OnKeyword, " "),
                this.Print(node.LeftExpression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.EqualsKeyword, " "),
                this.Print(node.RightExpression),
                node.Into != null 
                    ? Concat(SpaceIfNoPreviousComment,
                        this.PrintSyntaxToken(node.Into.IntoKeyword, " "),
                        this.PrintSyntaxToken(node.Into.Identifier)) 
                    : null
            );
        }
    }
}
