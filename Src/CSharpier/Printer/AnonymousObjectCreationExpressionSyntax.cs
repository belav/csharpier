using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectCreationExpressionSyntax(
            AnonymousObjectCreationExpressionSyntax node
        ) {
            return Docs.Group(
                this.PrintSyntaxToken(node.NewKeyword, Docs.Line),
                this.PrintSyntaxToken(node.OpenBraceToken),
                Docs.Indent(
                    Docs.Line,
                    this.PrintSeparatedSyntaxList(
                        node.Initializers,
                        this.PrintAnonymousObjectMemberDeclaratorSyntax,
                        Docs.Line
                    )
                ),
                Docs.Line,
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
        }
    }
}
