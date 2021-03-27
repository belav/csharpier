using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectCreationExpressionSyntax(
            AnonymousObjectCreationExpressionSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.NewKeyword, Line),
                this.PrintSyntaxToken(node.OpenBraceToken),
                Indent(
                    Line,
                    this.PrintSeparatedSyntaxList(
                        node.Initializers,
                        this.PrintAnonymousObjectMemberDeclaratorSyntax,
                        Line
                    )
                ),
                Line,
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
        }
    }
}
