using System;
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
                        or InterpolatedStringExpressionSyntax
                        or IsPatternExpressionSyntax
                        or LiteralExpressionSyntax
                        // TODO if we move the line into the indent (also change code in IndentIfNeeded) then all sorts of other formatting changes
                        // try to figure out how prettier does this with java/ts/js
                        // we may need conditional groups to get this to do what we want, bleh.
                        //or ObjectCreationExpressionSyntax
                        or QueryExpressionSyntax
                        or StackAllocArrayCreationExpressionSyntax;

            var groupId = Guid.NewGuid().ToString();

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
                            useLineInIndent
                                ? Doc.Null
                                : Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                            initializer.Value is InvocationExpressionSyntax
                                ? Doc.IndentIfBreak(
                                        Doc.Group(Node.Print(initializer.Value)),
                                        groupId
                                    )
                                : Doc.Group(IndentIfNeeded(initializer.Value, useLineInIndent))
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
