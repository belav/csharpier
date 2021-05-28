using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VariableDeclaration
    {
        public static Doc Print(VariableDeclarationSyntax node)
        {
            if (node.Variables.Count > 1)
            {
                var docs = Doc.Concat(
                    SeparatedSyntaxList.Print(
                        node.Variables,
                        VariableDeclarator.Print,
                        node.Parent is ForStatementSyntax ? Doc.Line : Doc.HardLine
                    )
                );

                return Doc.Concat(Node.Print(node.Type), " ", Doc.Indent(docs));
            }

            var variable = node.Variables[0];
            var initializer = variable.Initializer;

            var useLineInIndent =
                initializer?.Value
                    is BinaryExpressionSyntax
                        // TODO we might need IndentIfBreak for the edge cases on invocation and object creation
                        //or InvocationExpressionSyntax
                        or InterpolatedStringExpressionSyntax
                        or IsPatternExpressionSyntax
                        or LiteralExpressionSyntax
                        //or ObjectCreationExpressionSyntax
                        or QueryExpressionSyntax
                        or StackAllocArrayCreationExpressionSyntax;

            return Doc.Concat(
                Doc.Group(
                    Node.Print(node.Type),
                    " ",
                    Token.Print(variable.Identifier),
                    variable.ArgumentList != null
                        ? BracketedArgumentList.Print(variable.ArgumentList)
                        : Doc.Null,
                    initializer != null
                        ? Doc.Concat(" ", Token.Print(initializer.EqualsToken))
                        : Doc.Null
                ),
                initializer != null
                    ? Doc.Concat(
                            useLineInIndent ? Doc.Null : Doc.Group(Doc.Line),
                            Doc.Group(IndentIfNeeded(initializer.Value, useLineInIndent))
                        )
                    : Doc.Null
            );
        }

        private static Doc IndentIfNeeded(ExpressionSyntax initializerValue, bool useLineInIndent)
        {
            if (
                initializerValue
                    is AnonymousObjectCreationExpressionSyntax
                        or AnonymousMethodExpressionSyntax
                        or InitializerExpressionSyntax
                        or InvocationExpressionSyntax
                        or ConditionalExpressionSyntax
                        or ObjectCreationExpressionSyntax
                        or SwitchExpressionSyntax
            ) {
                return Node.Print(initializerValue);
            }

            return Doc.Indent(useLineInIndent ? Doc.Line : Doc.Null, Node.Print(initializerValue));
        }
    }
}
