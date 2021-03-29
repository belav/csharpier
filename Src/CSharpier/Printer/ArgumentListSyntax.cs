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
    }
}
