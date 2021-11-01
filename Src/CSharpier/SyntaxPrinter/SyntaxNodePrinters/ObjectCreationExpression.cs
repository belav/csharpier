using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ObjectCreationExpression
    {
        public static Doc Print(ObjectCreationExpressionSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            return Doc.Group(
                Token.PrintWithSuffix(node.NewKeyword, " "),
                Node.Print(node.Type),
                node.ArgumentList != null
                  ? Doc.GroupWithId(
                        groupId,
                        ArgumentListLike.Print(
                            node.ArgumentList.OpenParenToken,
                            node.ArgumentList.Arguments,
                            node.ArgumentList.CloseParenToken
                        )
                    )
                  : Doc.Null,
                node.Initializer != null
                  ? Doc.Concat(
                        node.ArgumentList != null ? Doc.IfBreak(" ", Doc.Line, groupId) : Doc.Line,
                        InitializerExpression.Print(node.Initializer)
                    )
                  : Doc.Null
            );
        }
    }
}
