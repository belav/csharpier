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
                node.Value is AnonymousObjectCreationExpressionSyntax
                || node.Value is AnonymousMethodExpressionSyntax
                || node.Value is ConditionalExpressionSyntax
                || node.Value is ObjectCreationExpressionSyntax
                || node.Value is InitializerExpressionSyntax
                || node.Value is ParenthesizedLambdaExpressionSyntax
                || node.Value is InvocationExpressionSyntax
                || node.Value is SwitchExpressionSyntax
            ) {
                separator = " ";
            }

            Doc result = Doc.Group(
                " ",
                Token.Print(node.EqualsToken, separator),
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
