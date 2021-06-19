using System;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class RightHandSide
    {
        public static Doc Print(ExpressionSyntax node)
        {
            var formatMode = node
                is AnonymousObjectCreationExpressionSyntax
                    or AnonymousMethodExpressionSyntax
                    or InitializerExpressionSyntax
                    or InvocationExpressionSyntax
                    or ConditionalExpressionSyntax
                    or ObjectCreationExpressionSyntax
                    or SwitchExpressionSyntax
                    or LambdaExpressionSyntax
                    or AwaitExpressionSyntax
                    or WithExpressionSyntax
                ? FormatMode.NoIndent
                : node
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
                formatMode is FormatMode.IndentWithLine
                    ? Doc.Null
                    : Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                node
                    is InvocationExpressionSyntax
                        or ParenthesizedLambdaExpressionSyntax
                        or ObjectCreationExpressionSyntax
                        or ElementAccessExpressionSyntax
                        or ArrayCreationExpressionSyntax
                        or ImplicitArrayCreationExpressionSyntax
                    ? Doc.IndentIfBreak(Doc.Group(Node.Print(node)), groupId)
                    : Doc.Group(IndentIfNeeded(node, formatMode))
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
