using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ArgumentList
    {
        // TODO I think this indent logic can move into the FlattenAndPrintNodes method
        // then hopefully it can share it with ShouldMergeFirstTwoGroups
        public static Doc Print(ArgumentListSyntax node)
        {
            return Doc.Group(
                Doc.IndentIf(
                    // indent if this is the first argumentList in a method chain
                    node.Parent
                        is InvocationExpressionSyntax
                        {
                            Expression: IdentifierNameSyntax
                                or MemberAccessExpressionSyntax
                                {
                                    Expression: ThisExpressionSyntax
                                        or PredefinedTypeSyntax
                                        or IdentifierNameSyntax
                                        {
                                            Identifier: { Text: { Length: <= 4 } }
                                        }
                                },
                            Parent: { Parent: InvocationExpressionSyntax }
                        },
                    ArgumentListLike.Print(
                        node.OpenParenToken,
                        node.Arguments,
                        node.CloseParenToken
                    )
                )
            );
        }
    }
}
