using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class EqualsValueClause
    {
        public static Doc Print(EqualsValueClauseSyntax node)
        {
            Doc separator = Doc.Line;
            if (node.Parent is PropertyDeclarationSyntax)
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
                Token.PrintWithSuffix(node.EqualsToken, separator),
                Node.Print(node.Value)
            );

            if (separator is LineDoc)
            {
                result = Doc.Indent(result);
            }

            return result;
        }
    }
}
