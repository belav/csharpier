using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEqualsValueClauseSyntax(EqualsValueClauseSyntax node)
        {
            var separator = Line;
            if (node.Value is AnonymousObjectCreationExpressionSyntax
                || node.Value is AnonymousMethodExpressionSyntax
                || node.Value is ConditionalExpressionSyntax
                || node.Value is ObjectCreationExpressionSyntax
                || node.Value is InitializerExpressionSyntax
                || node.Value is ParenthesizedLambdaExpressionSyntax
                || node.Value is InvocationExpressionSyntax
                || node.Value is SwitchExpressionSyntax
                || node.Value is QueryExpressionSyntax
            )
            {
                separator = SpaceIfNoPreviousComment;
            }
            
            var result = Group(
                // TODO GH-6 this should probably be line, but that breaks a ton of things
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.EqualsToken, separator),
                this.Print(node.Value));

            if (separator is LineDoc)
            {
                result = Indent(result);
            }

            return result;
        }
    }
}
