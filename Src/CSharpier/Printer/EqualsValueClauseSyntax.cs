using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEqualsValueClauseSyntax(EqualsValueClauseSyntax node)
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
                node.Value is AnonymousObjectCreationExpressionSyntax ||
                node.Value is AnonymousMethodExpressionSyntax ||
                node.Value is ConditionalExpressionSyntax ||
                node.Value is ObjectCreationExpressionSyntax ||
                node.Value is InitializerExpressionSyntax ||
                node.Value is ParenthesizedLambdaExpressionSyntax ||
                node.Value is InvocationExpressionSyntax ||
                node.Value is SwitchExpressionSyntax
            ) {
                separator = " ";
            }

            Doc result = Doc.Group(
                " ",
                this.PrintSyntaxToken(node.EqualsToken, separator),
                this.Print(node.Value)
            );

            if (separator is LineDoc)
            {
                result = Doc.Indent(result);
            }

            return result;
        }
    }
}
