using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ArgumentList
    {
        public static Doc Print(ArgumentListSyntax node)
        {
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Doc.Group(
                        ArgumentListLike.Print(
                            node.OpenParenToken,
                            node.Arguments,
                            node.CloseParenToken
                        )
                    )
                : ArgumentListLike.Print(
                        node.OpenParenToken,
                        node.Arguments,
                        node.CloseParenToken
                    );
        }
    }
}
