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

            var formatMode = initializer?.Value
                is AnonymousObjectCreationExpressionSyntax
                    or AnonymousMethodExpressionSyntax
                    or InitializerExpressionSyntax
                    or InvocationExpressionSyntax
                    or ConditionalExpressionSyntax
                    or ObjectCreationExpressionSyntax
                    or SwitchExpressionSyntax
                ? FormatMode.NoIndent
                : initializer?.Value
                        is BinaryExpressionSyntax
                            or InterpolatedStringExpressionSyntax
                            or IsPatternExpressionSyntax
                            or LiteralExpressionSyntax
                            or QueryExpressionSyntax
                            or StackAllocArrayCreationExpressionSyntax
                        ? FormatMode.IndentWithLine
                        : FormatMode.Indent;

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
                            formatMode is FormatMode.IndentWithLine
                                ? Doc.Null
                                : Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                            initializer.Value is InvocationExpressionSyntax
                                ? Doc.IndentIfBreak(
                                        Doc.Group(Node.Print(initializer.Value)),
                                        groupId
                                    )
                                : Doc.Group(IndentIfNeeded(initializer.Value, formatMode))
                        )
                    : Doc.Null
            );
        }

        private static Doc IndentIfNeeded(ExpressionSyntax initializerValue, FormatMode formatMode)
        {
            if (formatMode is FormatMode.NoIndent)
            {
                return Node.Print(initializerValue);
            }

            return Doc.Indent(
                formatMode is FormatMode.IndentWithLine ? Doc.Line : Doc.Null,
                Node.Print(initializerValue)
            );
        }

        private enum FormatMode
        {
            NoIndent,
            IndentWithLine,
            Indent
        }
    }
}
