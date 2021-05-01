using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectCreationExpressionSyntax(
            AnonymousObjectCreationExpressionSyntax node
        ) {
            return Doc.Group(
                this.PrintSyntaxToken(node.NewKeyword, Doc.Line),
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(
                        node.Initializers,
                        this.PrintAnonymousObjectMemberDeclaratorSyntax,
                        Doc.Line
                    )
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
        }
    }
}
