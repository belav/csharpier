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
            return Docs.Group(
                this.PrintSyntaxToken(node.NewKeyword, Docs.Line),
                SyntaxTokens.Print(node.OpenBraceToken),
                Docs.Indent(
                    Docs.Line,
                    SeparatedSyntaxList.Print(
                        node.Initializers,
                        this.PrintAnonymousObjectMemberDeclaratorSyntax,
                        Docs.Line
                    )
                ),
                Docs.Line,
                SyntaxTokens.Print(node.CloseBraceToken)
            );
        }
    }
}
