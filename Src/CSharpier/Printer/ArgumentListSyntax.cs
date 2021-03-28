using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentListSyntax(ArgumentListSyntax node)
        {
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Group(
                    PrintArgumentListLikeSyntax(
                        node.OpenParenToken,
                        node.Arguments,
                        node.CloseParenToken
                    )
                )
                : PrintArgumentListLikeSyntax(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken
                );
        }

        private Doc PrintArgumentListLikeSyntax(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<ArgumentSyntax> arguments,
            SyntaxToken closeParenToken) =>
            Concat(
                this.PrintSyntaxToken(openParenToken),
                arguments.Any()
                    ? Indent(
                        SoftLine,
                        this.PrintSeparatedSyntaxList(
                            arguments,
                            this.PrintArgumentSyntax,
                            Line
                        )
                    )
                    : Doc.Null,
                arguments.Any() ? SoftLine : Doc.Null,
                this.PrintSyntaxToken(closeParenToken)
            );
    }
}
