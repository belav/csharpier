using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class EqualsValueClause
{
    public static Doc Print(EqualsValueClauseSyntax node, PrintingContext context)
    {
        Doc separator = Doc.Line;
        if (
            node is
            {
                Parent: PropertyDeclarationSyntax,
                Value: not AnonymousObjectCreationExpressionSyntax
            }
        )
        {
            // keeping line
        }
        else if (node.Value is QueryExpressionSyntax)
        {
            separator = Doc.Null;
        }
        else if (
            node.Value
            is AnonymousObjectCreationExpressionSyntax
                or AnonymousMethodExpressionSyntax
                or ConditionalExpressionSyntax
                or ObjectCreationExpressionSyntax
                or InitializerExpressionSyntax
                or ParenthesizedLambdaExpressionSyntax
                or InvocationExpressionSyntax
                or SwitchExpressionSyntax
        )
        {
            separator = " ";
        }

        Doc result = Doc.Group(
            " ",
            Token.PrintWithSuffix(node.EqualsToken, separator, context),
            Node.Print(node.Value, context)
        );

        if (separator is LineDoc && node.Value is not CollectionExpressionSyntax)
        {
            result = Doc.Indent(result);
        }

        return result;
    }
}
